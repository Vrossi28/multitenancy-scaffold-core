using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email.Factories
{
    internal class ContactRequestedFactory : EmailFactory
    {
        private readonly IEmailBuilder _emailBuilder;

        public ContactRequestedFactory(IEmailBuilder emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        public override IEmailBuilder EmailBuilder => _emailBuilder;
        public override string Name => nameof(WelcomeFactory);
        public override async Task<object> Send(EmailModelRequest request)
        {
            var email = request.Attributes.GetValueOrDefault(EmailConstants.Email);
            var name = request.Attributes.GetValueOrDefault(EmailConstants.Name);
            var message = request.Attributes.GetValueOrDefault(EmailConstants.Message);
            var phone = request.Attributes.GetValueOrDefault(EmailConstants.Phone);

            var content = $"The user <strong>{name}</strong>, owning e-mail <strong>{email}</strong>, is requesting contact.";
            if(!string.IsNullOrWhiteSpace(email))
            { 
                content += $"<br/>With the following message: <strong>{message}</strong>";
            }

            var additionalMessage = "Please get in touch.";
            if (!string.IsNullOrWhiteSpace(phone))
            {
                additionalMessage += $"<br/>Phone number: <strong>{phone}</strong>";
            }

            return await _emailBuilder.Create("Contact Requested", request.Recipients)
                                .WithTemplateWithoutButton(content, additionalMessage)
                                .Send();
        }
    }
}
