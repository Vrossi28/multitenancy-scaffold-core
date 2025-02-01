using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Repositories
{
    public class Repository<TDbContext> : IDisposable
                where TDbContext : DbContext
    {
        protected readonly TDbContext _context;
        private bool _disposed;
        public Repository(TDbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _context?.Dispose();
            }

            _disposed = true;
        }
    }
    public abstract class Repository<TAggregateRoot, TDbContext> : Repository<TDbContext>, IRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot
        where TDbContext : DbContext
    {
        protected Repository(TDbContext context) : base(context)
        {
        }
        private DbSet<TAggregateRoot> DbSet => _context.Set<TAggregateRoot>();
        public virtual async Task<TAggregateRoot> FindById(Guid id) => await DbSet.FindAsync(id);
        public Task<bool> Exists(Guid id) => DbSet.AnyAsync(x => x.Id == id);
    }
}
