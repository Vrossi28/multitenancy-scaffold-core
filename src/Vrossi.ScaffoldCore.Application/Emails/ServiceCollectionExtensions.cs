using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Application.Emails.Commands;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Emails
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddEmailsUseCase(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<SendEmail, Response>, SendEmail.Handler>();
        }
    }
}
