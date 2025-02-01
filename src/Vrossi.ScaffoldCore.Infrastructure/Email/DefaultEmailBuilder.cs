using Vrossi.ScaffoldCore.Infrastructure.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    internal class DefaultEmailBuilder : IEmailBuilder, IEmailContentBuilder, IEmailTemplateBuilder
    {
        private readonly IEmailSender _emailSender;
        private EmailIdentity _identity;
        private EmailContent _content;
        public DefaultEmailBuilder(IEmailSender emailSender, IOptions<EmailIdentity> identity)
        {
            _emailSender = emailSender;
            _identity = identity.Value;
        }
        public IEmailTemplateBuilder Create(string subject, List<string> destinations)
        {
            _content = new EmailContent()
            {
                Subject = subject,
                Emails = destinations
            };

            return this;
        }
        public IEmailTemplateBuilder Create(string subject, string destination) => Create(subject, new List<string>() { destination });
        public IEmailContentBuilder WithDefaultTemplate(string message, string additionalMessage, string linkText, string linkUrl, string title = null)
        {
            var body = Resources.DefaultEmailTemplate.Replace("##TITLE##", title ?? _content.Subject)
                                          .Replace("##MESSAGE##", message)
                                          .Replace("##ADDITIONAL_MESSAGE##", additionalMessage)
                                          .Replace("##LINK_TEXT##", linkText)
                                          .Replace("##LINK_URL##", linkUrl);

            _content.SetBody(body);

            return this;
        }
        public IEmailContentBuilder WithTemplateWithoutButton(string message, string additionalMessage, string title = null)
        {
            var body = Resources.DefaultEmailTemplateWithoutButton.Replace("##TITLE##", title ?? _content.Subject)
                                          .Replace("##MESSAGE##", message)
                                          .Replace("##ADDITIONAL_MESSAGE##", additionalMessage);

            _content.SetBody(body);

            return this;
        }
        public async Task<bool> Send() => await _emailSender.Send(_identity, _content);
    }
}
