using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Type SecurityTokenExpiredExceptionType = typeof(SecurityTokenExpiredException);
        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtTokenSection = configuration.GetSection("JwtToken");
            services.Configure<JwtTokenOptions>(jwtTokenSection);
            services.AddSingleton<ITokenProvider, JwtAuthManager>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                          .AddJwtBearer(options =>
                          {
                              options.TokenValidationParameters = new TokenValidationParameters
                              {
                                  ValidateLifetime = true,
                                  RequireExpirationTime = true,
                                  ClockSkew = TimeSpan.Zero,

                                  ValidateIssuer = true,
                                  ValidateAudience = true,
                                  ValidateIssuerSigningKey = true,
                                  ValidIssuer = jwtTokenSection["Issuer"],
                                  ValidAudience = jwtTokenSection["Audience"],
                                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSection["SecretKey"]))
                              };

                              options.Events = new JwtBearerEvents
                              {
                                  OnChallenge = async context =>
                                  {
                                      context.HandleResponse();

                                      context.Response.ContentType = MediaTypeNames.Application.Json;
                                      context.Response.StatusCode = StatusCodes.Status401Unauthorized; 

                                      if (string.IsNullOrEmpty(context.Error))
                                          context.Error = "invalid_token";
                                      if (string.IsNullOrEmpty(context.ErrorDescription))
                                          context.ErrorDescription = "The request needs a valid JWT";

                                      if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == SecurityTokenExpiredExceptionType)
                                      {
                                          var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                                          context.Response.Headers.Add("Token-Expired", "true");
                                          context.ErrorDescription = $"Token expired at {authenticationException.Expires:o}";
                                      }

                                      var errorDetails = new ErrorDetails()
                                      {
                                          StatusCode = context.Response.StatusCode,
                                          Message = "Unauthorized"
                                      };

                                      await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
                                  }
                              };
                          });
        }
    }
}
