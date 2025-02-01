using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Mappings
{
    internal class TenantMapping : AggregateRootTypeConfiguration<Tenant>
    {
        public override void ConfigureCore(EntityTypeBuilder<Tenant> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
        }
    }
}
