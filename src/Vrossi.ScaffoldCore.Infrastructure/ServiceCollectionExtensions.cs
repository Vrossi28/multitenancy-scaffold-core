using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Repositories;
using Vrossi.ScaffoldCore.Infrastructure.Email;

namespace Vrossi.ScaffoldCore.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepositories();
            services.AddDbContext<ScaffoldCoreContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString(InfrastructureContants.ConnectionString);
                options.UseNpgsql(connectionString,
                    builder => builder.MigrationsAssembly(typeof(ScaffoldCoreContext).Assembly.FullName));
            });
            services.AddEmailSender(configuration);

            return services;
        }
    }
    
}
