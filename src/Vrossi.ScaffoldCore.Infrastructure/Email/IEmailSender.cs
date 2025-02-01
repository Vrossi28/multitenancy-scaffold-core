using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    internal interface IEmailSender
    {
        Task<bool> Send(EmailIdentity identity, EmailContent content);
    }
}
