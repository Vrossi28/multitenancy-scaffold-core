using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    internal class EmailService : IEmailSender
    {
        private readonly Func<SmtpClient> _smtpClientFactory;
        private readonly SemaphoreSlim _semaphore;

        public EmailService(Func<SmtpClient> smtpClientFactory)
        {
            _smtpClientFactory = smtpClientFactory;
            _semaphore = new SemaphoreSlim(1);
        }

        public async Task<bool> Send(EmailIdentity identity, EmailContent content)
        {
            
            if (identity == null)
                throw new NullReferenceException(nameof(identity));

            if (content == null)
                throw new NullReferenceException(nameof(content));

            identity.EnsureToSend();
            content.EnsureToSend();

            await _semaphore.WaitAsync();

            try
            {
                using var client = _smtpClientFactory();
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(identity.SenderEmail, identity.DisplayName),
                    Subject = content.Subject,
                    Body = content.Body,
                    IsBodyHtml = true
                };

                foreach (var recipient in content.Emails)
                {
                    mailMessage.To.Add(new MailAddress(recipient));
                }
                foreach (var recipient in content.CC)
                {
                    mailMessage.CC.Add(new MailAddress(recipient));
                }

                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                throw new EmailSendingFailureException(ex.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
