using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Accounts.Queries;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Accounts
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddAccountsUseCase(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<PerformLogin, Response<Account>>, PerformLogin.Handler>();
            services.AddScoped<IRequestHandler<CreateAccount, Response<AccountDto>>, CreateAccount.Handler>();
            services.AddScoped<IRequestHandler<GetLoggedAccount, Response<AccountDto>>, GetLoggedAccount.Handler>();
            services.AddScoped<IRequestHandler<ChangeDefault, Response>, ChangeDefault.Handler>();
            services.AddScoped<IRequestHandler<CreateProfile, Response<AccountDto>>, CreateProfile.Handler>();
        }
    }
}
