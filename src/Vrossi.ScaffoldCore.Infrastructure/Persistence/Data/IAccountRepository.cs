using Vrossi.ScaffoldCore.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        bool ResetTokenAlreadyExists(string resetToken);
        Task<Account> FindByEmail(string email);
        Task<Account> GetByIdWithDefaultAndProfiles(Guid id);
    }
}
