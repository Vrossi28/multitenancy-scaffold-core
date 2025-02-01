using Hangfire;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    internal class HangfireJobScheduler : IHangfireJobScheduler
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HangfireJobScheduler(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        public Task Enqueue<TPayload>(TPayload payload) where TPayload : class
        {
            if (payload is EmailModelRequest)
                _backgroundJobClient.Enqueue<EmailJob>(job => job.Handle(null, payload));

            return Task.CompletedTask;
        }
    }
}
