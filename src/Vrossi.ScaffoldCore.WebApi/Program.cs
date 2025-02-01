using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Application;
using Vrossi.ScaffoldCore.Infrastructure;
using Vrossi.ScaffoldCore.Infrastructure.Hangfire;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Authorization;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Swagger;
using Npgsql;
using Polly;
using System.Text.Encodings.Web;
using System.Text.Json;
using static CSharpFunctionalExtensions.Result;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Multitenancy;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System.Security.Claims;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire;

var builder = WebApplication.CreateBuilder(args);
var typeOfProgram = typeof(Program);

builder.Configuration.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var serializerOptions = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

// Add services to the container.

builder.Services.AddSingleton(serializerOptions);
builder.Services.AddCors();
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization();
builder.Services.AddMediator(typeOfProgram)
                .AddTransactionBehavior();

builder.Host.ConfigureAppConfiguration(options => options.AddEnvironmentVariables());

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
});

builder.Services.AddControllers(options =>
                {
                    options.AddAuthorizeTenantFilter();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = serializerOptions.PropertyNamingPolicy;

                    foreach (var converter in serializerOptions.Converters)
                        options.JsonSerializerOptions.Converters.Add(converter);
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = CustomModelStateValidator.ValidateModelState;
                });
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddTenantStrategy<DefaultStrategy>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(serviceProvider => serviceProvider.GetService<IHttpContextAccessor>().HttpContext?.User ?? Thread.CurrentPrincipal as ClaimsPrincipal);

builder.Services.AddHangfireMvc(builder.Configuration, builder.Environment);
builder.Services.AddHangfireInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseHttpLogging();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var policy = Policy
            .Handle<NpgsqlException>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, _, retryAttempt, _) =>
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning($"Retry #{retryAttempt} due to: {exception.Message}");
                });
    await policy.ExecuteAsync(() =>
    {
        try
        {
            var context = services.GetRequiredService<ScaffoldCoreContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
        catch (NpgsqlException ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Failed to connect to the database.");
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }

        return Task.CompletedTask;
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseRouting();
app.UseCookiePolicy();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins:Origins").Get<string[]>();

app.UseCors(builder =>
{
    builder.WithOrigins(allowedOrigins)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseJobScheduler();

app.Run();
