using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email.Factories
{
    internal class WelcomeFactory : EmailFactory
    {
        private readonly IEmailBuilder _emailBuilder;

        private const string CONTACT_RECEIVED_MESSAGE = "Olá! Muito obrigado por seu contato, em breve um de nossos especialistas irá entrar em contato com você.";

        public WelcomeFactory(IEmailBuilder emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        public override IEmailBuilder EmailBuilder => _emailBuilder;
        public override string Name => nameof(WelcomeFactory);
        public override async Task<object> Send(EmailModelRequest request)
        {
            var additionalMessage = "Por enquanto, você pode verificar nosso blog de conteúdos em https://mosaic-ai.io/blog";
            return await _emailBuilder.Create("Bem-vindo a mosaic.ai", request.Recipients)
                                .WithTemplateWithoutButton(CONTACT_RECEIVED_MESSAGE, additionalMessage)
                                .Send();
        }
    }
}
