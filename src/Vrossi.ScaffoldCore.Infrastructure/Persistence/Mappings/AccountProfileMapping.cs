using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Mappings
{
    internal class AccountProfileMapping : EntityTypeConfiguration<AccountProfile>
    {
        private const string TenantId = "TenantId";
        public override void ConfigureCore(EntityTypeBuilder<AccountProfile> builder)
        {
            builder.Property(x => x.Profile).IsRequired();
            builder.HasOne(x => x.Tenant).WithMany().HasForeignKey(TenantId).OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(AccountProfile.Tenant)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
