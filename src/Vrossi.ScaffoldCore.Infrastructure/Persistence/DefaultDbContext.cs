using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Messages;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence
{
    public abstract class DefaultDbContext<T> : DbContext
        where T : DbContext
    {
        private const string UpdatedAt = "UpdatedAt";

        protected DefaultDbContext(DbContextOptions<T> options) : base(options) {}
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeModifiedDateOnModifiedState(UpdatedAt);
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ConfigureUtcDateTime();
            modelBuilder.SetNonUnicodeInAllStrings();

            modelBuilder.Ignore<DomainEvent>();
            modelBuilder.Ignore<ApplicationEvent>();

            OnModelCreatingCore(modelBuilder);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            ArgumentNullException.ThrowIfNull(configurationBuilder);

            configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
            configurationBuilder.Properties<DateTime?>().HaveConversion<UtcNullableDateTimeConverter>();
        }
        public virtual void OnModelCreatingCore(ModelBuilder modelBuilder) { }
    }
}
