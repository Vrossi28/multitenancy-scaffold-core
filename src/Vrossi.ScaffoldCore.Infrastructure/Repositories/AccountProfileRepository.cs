using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Repositories
{
    public class AccountProfileRepository : Repository<AccountProfile, ScaffoldCoreContext>, IAccountProfileRepository
    {
        public AccountProfileRepository(ScaffoldCoreContext context) : base(context)
        {
        }

        public async Task AddAsync(AccountProfile item) => await _context.AccountProfiles.AddAsync(item);

        public void Delete(AccountProfile item) => _context.AccountProfiles.Remove(item);

        public void Update(AccountProfile item) => _context.AccountProfiles.Update(item);
    }
}
