using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public interface ITenantRepository : IBaseRepository<Tenant>
    {
        Task<PaginationResult<Tenant>> List(IPaginationRequest request);
    }
}
