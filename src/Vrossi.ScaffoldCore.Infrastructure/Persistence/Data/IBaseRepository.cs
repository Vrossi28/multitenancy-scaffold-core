using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public interface IBaseRepository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        Task AddAsync(TAggregateRoot item);
        void Update(TAggregateRoot item);
        void Delete(TAggregateRoot item);
    }
}
