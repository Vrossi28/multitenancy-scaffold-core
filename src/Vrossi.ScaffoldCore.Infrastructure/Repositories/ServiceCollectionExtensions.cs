using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Repositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IAccountProfileRepository, AccountProfileRepository>();

            return services;
        }
    }
}
