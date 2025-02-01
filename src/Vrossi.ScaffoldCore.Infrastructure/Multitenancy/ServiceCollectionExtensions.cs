using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Multitenancy
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTenantStrategy<TStrategy>(this IServiceCollection services)
            where TStrategy : class, ITenantStrategy
        {
            services.AddTransient<ITenantStrategy, TStrategy>();
        }
    }
}
