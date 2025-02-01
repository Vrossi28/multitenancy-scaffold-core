using Hangfire.Server;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using Hangfire.Console;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email
{
    [AutomaticRetry(Attempts = 3)]
    internal class EmailJob : HangfireJob
    {
        private readonly IEmailStrategy _emailStrategy;
        public EmailJob(IEmailStrategy emailStrategy)
        {
            _emailStrategy = emailStrategy;
        }

        public override string Name => "Email Job";
        protected override async Task OnExecuting(PerformContext context, params object[] args)
        {
            try
            {
                var json = args[0]?.ToString();
                var request = JsonSerializer.Deserialize<EmailModelRequest>(json);

                var emailFactory = _emailStrategy.CreateInputFactory(request.ModelName);

                context.WriteLine($"EmailFactory: {emailFactory.Name}");

                var sent = await emailFactory.Send(request);

                if (sent is null || !(bool)sent)
                    throw new HangfireJobException("Email could not be sent");

                context.WriteLine("Job Completed");
            }
            catch (Exception ex)
            {
                context.WriteError($"An exception occurred: {ex.Message}");
                throw new HangfireJobException(ex.Message);
            }
        }
    }
}
