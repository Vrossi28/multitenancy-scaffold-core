using AutoMapper;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Tenants
{
    public class TenantsProfile : Profile
    {
        public TenantsProfile()
        {
            CreateMap<Tenant, TenantDto>();
            CreateMap<PaginationResult<Tenant>, TenantPaginatedListDto>();
        }
    }
}
