using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email
{
    internal class EmailStrategy : IEmailStrategy
    {
        private readonly IEmailBuilder _emailBuilder;

        public EmailStrategy(IEmailBuilder emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }
        public EmailFactory CreateInputFactory(EmailModel emailModel)
        {
            if (emailModel == EmailModel.Welcome)
                return new WelcomeFactory(_emailBuilder);

            if (emailModel == EmailModel.ContactRequested)
                return new ContactRequestedFactory(_emailBuilder);

            throw new Exception("Is not a valid name for type creation of a EmailFactory");
        }
    }
    internal interface IEmailStrategy
    {
        EmailFactory CreateInputFactory(EmailModel emailModel);
    }
}
