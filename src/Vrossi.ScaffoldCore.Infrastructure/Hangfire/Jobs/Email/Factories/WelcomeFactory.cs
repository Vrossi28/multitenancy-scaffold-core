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

        private const string CONTACT_RECEIVED_MESSAGE = "Hello! Thank you very much for requesting our contact, very soon one of our specialists with get in touch with you.";

        public WelcomeFactory(IEmailBuilder emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        public override IEmailBuilder EmailBuilder => _emailBuilder;
        public override string Name => nameof(WelcomeFactory);
        public override async Task<object> Send(EmailModelRequest request)
        {
            var additionalMessage = "In the meantime, you can follow me at https://github.com/vrossi28";
            return await _emailBuilder.Create("Welcome to Vrossi28", request.Recipients)
                                .WithTemplateWithoutButton(CONTACT_RECEIVED_MESSAGE, additionalMessage)
                                .Send();
        }
    }
}
