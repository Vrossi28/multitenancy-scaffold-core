using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.Domain;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Mappings
{
    internal class AccountMapping : AggregateRootTypeConfiguration<Account>
    {
        private const string AccountId = "AccountId";
        public override void ConfigureCore(EntityTypeBuilder<Account> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(255).IsRequired();
            builder.Property(x => x.LastLoginDate);

            builder.HasMany(x => x.Profiles).WithOne().HasForeignKey(AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(Account.Profiles)).SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(x => x.Email).HasDatabaseName("IX_UNIQUE_Account_Email").IsUnique();
        }
    }
}
