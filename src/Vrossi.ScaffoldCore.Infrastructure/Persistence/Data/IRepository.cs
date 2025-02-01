using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public interface IRepository<TAggregateRoot> : IDisposable
     where TAggregateRoot : AggregateRoot
    {
        Task<TAggregateRoot> FindById(Guid id);
        Task<bool> Exists(Guid id);
    }
}
