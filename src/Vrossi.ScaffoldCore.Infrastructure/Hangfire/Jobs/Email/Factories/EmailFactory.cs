using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email.Factories
{
    internal abstract class EmailFactory
    {
        public abstract IEmailBuilder EmailBuilder { get; }
        public abstract string Name { get; }
        public abstract Task<object> Send(EmailModelRequest request);
    }
}
