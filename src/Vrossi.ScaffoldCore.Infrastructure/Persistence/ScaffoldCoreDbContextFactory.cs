using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence
{
    public class ScaffoldCoreContextFactory : IDesignTimeDbContextFactory<ScaffoldCoreContext>
    {
        public ScaffoldCoreContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            var builder = new DbContextOptionsBuilder<ScaffoldCoreContext>();
            var connectionString = configuration.GetConnectionString(InfrastructureContants.ConnectionString);

            builder.UseNpgsql(connectionString);

            return new ScaffoldCoreContext(builder.Options);
        }
    }
}
