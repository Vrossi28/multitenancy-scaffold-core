using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using Vrossi.ScaffoldCore.Infrastructure.Hangfire.Jobs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHangfireInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailStrategy, EmailStrategy>();
            services.AddScoped<IHangfireEmailManager, HangfireEmailManager>();
            services.AddScoped<IHangfireJobScheduler, HangfireJobScheduler>();
        }
    }
}
