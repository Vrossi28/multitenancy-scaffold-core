using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Mappings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CSharpFunctionalExtensions.Result;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence
{
    public class ScaffoldCoreContext : DefaultDbContext<ScaffoldCoreContext>
    {
        IConfiguration _configuration;
        public ScaffoldCoreContext(DbContextOptions<ScaffoldCoreContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public ScaffoldCoreContext(DbContextOptions<ScaffoldCoreContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if(_configuration is not null)
                {
                    optionsBuilder.UseNpgsql(_configuration.GetConnectionString(InfrastructureContants.ConnectionString));
                }
            }
        }

        public override void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new AccountProfileMapping());
        }
    }
}
