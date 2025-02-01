using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    public abstract class AggregateRootTypeConfiguration<TAggregateRoot> : EntityTypeConfiguration<TAggregateRoot>
        where TAggregateRoot : AggregateRoot
    {
        public override void Configure(EntityTypeBuilder<TAggregateRoot> builder)
        {
            base.Configure(builder);
        }
    }
}
