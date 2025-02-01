using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire
{
    public interface IHangfireJobScheduler
    {
        Task Enqueue<TPayload>(TPayload payload) where TPayload : class;
    }
}
