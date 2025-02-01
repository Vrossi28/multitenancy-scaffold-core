using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Vrossi.ScaffoldCore.Application.Accounts;
using Vrossi.ScaffoldCore.Application.Emails;
using Vrossi.ScaffoldCore.Application.Tenants;
using Vrossi.ScaffoldCore.Infrastructure.Extensions;
using System.Text;

namespace Vrossi.ScaffoldCore.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            #region HealthCheck
            //services.AddHealthChecks()
            //    .AddCheck<DatabaseHealthCheck>("Database");
            #endregion

            #region Hangfire
            //services.AddHangfire(setup => setup.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            //services.AddHangfireServer();
            //services.AddTransient<HangfireJobActivator>();
            //var serviceProvider = services.BuildServiceProvider();
            //GlobalConfiguration.Configuration.UseActivator<HangfireJobActivator>(new HangfireJobActivator(serviceProvider));
            #endregion

            #region Attachments
            //services.AddScoped<IStorageService, StorageService>();
            //services.Configure<FormOptions>(options =>
            //{
            //    options.ValueCountLimit = int.MaxValue;
            //    options.MultipartBodyLengthLimit = int.MaxValue;
            //    options.MemoryBufferThreshold = int.MaxValue;
            //});
            #endregion

            #region Auto Mapper
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
            #endregion

            #region MediatR
            services.AddAccountsUseCase();
            services.AddTenantsUseCase();
            services.AddEmailsUseCase();
            #endregion
            return services;
        }
    }
}
