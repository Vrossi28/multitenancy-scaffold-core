using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.Domain;

namespace Vrossi.ScaffoldCore.UnitTest.Fixtures
{
    public class DomainFixtures
    {
        public static Account GetAccount(string name = "Vinicius", string lastName = "Rossi", string email = "vinicius@test.com", string password = "Password", bool admin = false, Tenant tenant = null)
        {
            if (tenant is null)
                return new Account(name, lastName, email, password);

            return new Account(name, lastName, email, password, admin, tenant);
        }

        public static Tenant GetTenant(string name = "Tenant") => new Tenant(name);
    }
}
