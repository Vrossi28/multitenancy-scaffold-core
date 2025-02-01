using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    public static class HangfireJobExtensions
    {
        private const string TenantId = "TenantId";
        public static Guid GetTenantId(this PerformContext performContext)
        {
            if (performContext == null)
                throw new ArgumentNullException(nameof(performContext));

            return performContext.GetJobParameter<Guid>(TenantId);
        }
    }
}
