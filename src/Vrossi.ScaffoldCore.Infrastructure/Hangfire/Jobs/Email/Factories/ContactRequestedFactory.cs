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

            var content = $"O usuário <strong>{name}</strong>, de email <strong>{email}</strong>, está solicitando contato.";
            if(!string.IsNullOrWhiteSpace(email))
            { 
                content += $"<br/>Com a seguinte mensagem: <strong>{message}</strong>";
            }

            var additionalMessage = "Por favor entre em contato.";
            if (!string.IsNullOrWhiteSpace(phone))
            {
                additionalMessage += $"<br/>Telefone: <strong>{phone}</strong>";
            }

            return await _emailBuilder.Create("Contato Solicitado", request.Recipients)
                                .WithTemplateWithoutButton(content, additionalMessage)
                                .Send();
        }
    }
}
