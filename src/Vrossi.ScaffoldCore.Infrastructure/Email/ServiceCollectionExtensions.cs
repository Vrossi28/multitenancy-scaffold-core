using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var emailIdentity = new EmailIdentity();
            configuration.GetSection("EmailService").Bind(emailIdentity);

            services.Configure<EmailIdentity>(options =>
            {
                options.SmtpPort = emailIdentity.SmtpPort;
                options.SmtpServer = emailIdentity.SmtpServer;
                options.SenderPassword = emailIdentity.SenderPassword;
                options.SenderEmail = emailIdentity.SenderEmail;
                options.ContactRecipients = emailIdentity.ContactRecipients;
                options.DisplayName = emailIdentity.DisplayName;
            });

            services.AddSingleton<Func<SmtpClient>>(serviceProvider => () => new SmtpClient(emailIdentity.SmtpServer)
            {
                Port = emailIdentity.SmtpPort,
                Credentials = new NetworkCredential(emailIdentity.SenderEmail, emailIdentity.SenderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            });

            services.AddSingleton<IEmailSender>(serviceProvider =>
            {
                var smtpClientFactory = serviceProvider.GetRequiredService<Func<SmtpClient>>();
                return new EmailService(smtpClientFactory);
            });

            services.AddTransient<IEmailBuilder, DefaultEmailBuilder>();
        }
    }
}
