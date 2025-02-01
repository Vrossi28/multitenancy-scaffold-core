using Hangfire.Console;
using Hangfire.Server;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    public abstract class HangfireJob : IHangfireJob
    {
        public abstract string Name { get; }
        public async Task Handle(PerformContext context, params object[] args)
        {
            try
            {
                context.WriteLine($"Starting Job \"{Name}\"...");
                await OnExecuting(context, args);
                context.WriteSucess($"Job \"{Name}\" successfully completed!");
            }
            catch (HangfireJobException jobEx)
            {
                await OnHangfireJobException(context, jobEx);
            }
            catch (Exception ex)
            {
                context.WriteError($"Error in Job {Name}: {ex.Message}");
                throw;
            }
        }
        protected abstract Task OnExecuting(PerformContext context, params object[] args);
        public virtual Task OnHangfireJobException(PerformContext context, HangfireJobException hangfireJobException) => Task.CompletedTask;
    }
    public interface IHangfireJob
    {
        string Name { get; }
        Task Handle(PerformContext context, params object[] args);
    }

    public class HangfireJobException : Exception
    {
        public Dictionary<string, object> Items { get; set; }
        public HangfireJobException(string message, Dictionary<string, object> items = null) : base(message)
        {
            Items = items ?? new Dictionary<string, object>();
        }
        public HangfireJobException(IHangfireResponse hangfireResponse, Dictionary<string, object> items = null) : this(hangfireResponse.Message, items) { }
    }
}
