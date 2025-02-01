using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static void SetNonUnicodeInAllStrings(this ModelBuilder modelBuilder)
        {
            var mutableProperties = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties());
            var stringMutableProperties = mutableProperties.Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null);

            foreach (var mutableProperty in stringMutableProperties)
            {
                mutableProperty.SetIsUnicode(false);
            }
        }

        public static async Task<int> SaveChangesIfHasChanges(this DbContext dbContext, CancellationToken cancellationToken = default)
        {
            if (dbContext.ChangeTracker.HasChanges())
                return await dbContext.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
