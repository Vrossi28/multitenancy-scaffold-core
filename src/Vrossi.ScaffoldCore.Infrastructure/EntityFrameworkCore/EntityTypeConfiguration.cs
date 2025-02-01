using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
           where TEntity : Entity
    {
        private const string TenantId = "TenantId";
        public virtual string TableName => typeof(TEntity).Name;
        public virtual string Schema => "dbo";
        public virtual bool DefineKeys => true;
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(TableName, Schema);

            if (DefineKeys)
                ConfigureKeys(builder);

            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();


            builder.Ignore(x => x.IsNew);

            ConfigureCore(builder);

            builder.ApplyTenantProperty(TenantId);
        }
        protected virtual void ConfigureKeys(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
        public abstract void ConfigureCore(EntityTypeBuilder<TEntity> builder);
    }
}
