using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account, ScaffoldCoreContext>, IAccountRepository
    {
        public AccountRepository(ScaffoldCoreContext context) : base(context)
        {
        }

        public async Task AddAsync(Account account) => await _context.Accounts.AddAsync(account);
        public void Delete(Account account) => _context.Accounts.Remove(account);
        public void Update(Account account) => _context.Accounts.Attach(account);
        public async Task<Account> GetByIdWithDefaultAndProfiles(Guid id) => await _context.Accounts.Include(x => x.Default).Include(x => x.Profiles).ThenInclude(x => x.Tenant).SingleOrDefaultAsync(x => x.Id == id);

        public bool ResetTokenAlreadyExists(string resetToken) => _context.Accounts.Where((a) => a.PasswordResetToken == resetToken).Any();

        public async Task<Account> FindByEmail(string email) => await _context.Accounts.Include(x => x.Profiles).ThenInclude(x => x.Tenant).Include(x => x.Default).Where(a => a.Email.ToLower() == email.ToLower()).SingleOrDefaultAsync();
    }
}
