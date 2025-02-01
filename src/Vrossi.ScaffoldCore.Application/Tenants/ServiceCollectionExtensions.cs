using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Queries;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Tenants
{
    internal static class ServiceCollectionExtensions
    {
        public static void AddTenantsUseCase(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<RetrievePaginatedList, Response<TenantPaginatedListDto>>, RetrievePaginatedList.Handler>();
            services.AddScoped<IRequestHandler<CreateTenant, Response<TenantDto>>, CreateTenant.Handler>();
        }
    }
}
