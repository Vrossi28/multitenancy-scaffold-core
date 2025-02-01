using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Swagger.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOptions = configuration.GetSection("Swagger").Get<SwaggerSettings>() ?? SwaggerSettings.Default;

            if (!swaggerOptions.Enabled || swaggerOptions.Versions.Count == 0)
                return;

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<XTenantOperationFilter>();
                options.CustomSchemaIds(type => type.ToString());

                foreach (var version in swaggerOptions.Versions)
                {
                    options.SwaggerDoc(version.Version, new OpenApiInfo
                    {
                        Title = version.Title,
                        Description = version.Description,
                        Version = version.Version,
                        Contact = new OpenApiContact()
                        {
                            Name = version.Contact.Name,
                            Email = version.Contact.Email
                        }
                    });
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Token Authorization Header using Bearer Scheme. Example: Authorization: \"Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };

                var securitySchemeReference = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement { { securitySchemeReference, Array.Empty<string>() } });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);
            });
        }
        public static void UseSwagger(this WebApplication app)
        {
            var swaggerOptions = app.Configuration.GetSection("Swagger").Get<SwaggerSettings>() ?? SwaggerSettings.Default;

            if (!swaggerOptions.Enabled || swaggerOptions.Versions.Count == 0)
                return;

            SwaggerBuilderExtensions.UseSwagger(app);

            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = string.Empty;

                foreach (var version in swaggerOptions.Versions)
                {
                    opt.SwaggerEndpoint(version.Endpoint, version.Title);
                    opt.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                }
            });
        }
    }
}
