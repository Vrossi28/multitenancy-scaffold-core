using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore.Pagination;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Repositories
{
    internal class TenantRepository : Repository<Tenant, ScaffoldCoreContext>, ITenantRepository
    {
        public TenantRepository(ScaffoldCoreContext context) : base(context)
        {

        }
        public override Task<Tenant> FindById(Guid id) => _context.Tenants.Where(x => x.Id == id).FirstOrDefaultAsync();
        public async Task AddAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesIfHasChanges();
        }
        public void Update(Tenant tenant) => _context.Tenants.Attach(tenant);
        public void Delete(Tenant tenant) => _context.Tenants.Remove(tenant);
        public Task<PaginationResult<Tenant>> List(IPaginationRequest request)
        {
            IQueryable<Tenant> query = _context.Tenants;

            if (!request.IsAdmin)
                query = query.Where(x => request.AllowedTenantIds.Contains(x.Id));

            if (request.TenantId != null)
                query = query.Where(x => x.Id.Equals(request.TenantId));

            return query.GetPagination(request);
        }
    }
}
