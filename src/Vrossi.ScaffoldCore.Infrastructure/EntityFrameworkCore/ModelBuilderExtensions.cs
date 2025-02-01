using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void ConfigureUtcDateTime(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetColumnType("timestamp with time zone");
                        property.SetValueConverter(new UtcDateTimeConverter());
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("timestamp with time zone");
                        property.SetValueConverter(new UtcNullableDateTimeConverter());
                    }
                }
            }
        }
    }
}
