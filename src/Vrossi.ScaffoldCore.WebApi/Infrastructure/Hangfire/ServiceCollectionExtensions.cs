using Hangfire.Console;
using Hangfire;
using Microsoft.Extensions.Options;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authentication;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authorization;
using Hangfire.MemoryStorage;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire
{
    internal static class ServiceCollectionExtensions
    {
        private const string DEFAULT_CULTURE = "en-US";

        private static readonly ConsoleOptions _consoleOptions = new() { BackgroundColor = "#0d3163" };
        public static void AddHangfireMvc(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var hangfireSection = configuration.GetSection("Hangfire");

            services.Configure<HangfireOptions>(hangfireSection);

            services.AddHangfire((serviceProvider, config) =>
            {
                config.UseStorage(configuration, environment);

                config.UseConsole(_consoleOptions);

                config.UseFilter(new HangfireCultureFilter() { Culture = DEFAULT_CULTURE, UiCulture = DEFAULT_CULTURE });
                config.UseFilter(new HangfireAuthenticationFilter(serviceProvider.GetService<ITenantStrategy>()));
            });

            services.AddHangfireServer();

            services.AddTransient<JobActivator, CustomJobActivator>();
            services.AddTransient<HangfireAuthenticationFilter>();
        }
        public static void UseJobScheduler(this WebApplication app)
        {
            GlobalConfiguration.Configuration.UseActivator(new CustomJobActivator(app.Services.GetRequiredService<IServiceScopeFactory>()));

            var hangfireOptions = app.Services.GetService<IOptions<HangfireOptions>>().Value;

            var users = new List<HangfireUser>() { new HangfireUser()
            {
                Login = hangfireOptions.UserName,
                PasswordClear = hangfireOptions.Password
            } };

            var authorizationOptions = new HangfireAuthorizationOptions() { Users = users };

            app.UseHangfireDashboard(hangfireOptions.PathMath, new DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                AppPath = hangfireOptions.AppPath,
                DashboardTitle = hangfireOptions.DashboardTitle,
                Authorization = new[] { new HangfireAuthorizationFilter(authorizationOptions) }
            });
        }
    }
    internal static class HangfireStorageExtensions
    {
        public static IGlobalConfiguration UseStorage(this IGlobalConfiguration config, IConfiguration configuration, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                config.UseMemoryStorage();
            }
            else
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
                config.UseRedisStorage(connectionMultiplexer);
            }

            return config;
        }
    }
}
