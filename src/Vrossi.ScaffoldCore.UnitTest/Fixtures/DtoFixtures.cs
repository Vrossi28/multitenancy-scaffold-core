using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Models;

namespace Vrossi.ScaffoldCore.UnitTest.Fixtures
{
    public class DtoFixtures
    {
        public static TenantDto GetTenant(string name = "Tenant") => new TenantDto { Id = Guid.NewGuid(), Name = name };
        public static AccountDto GetAccount(string name = "Vinicius", 
            string lastName = "Rossi", 
            string email = "vrossi@test.com", 
            string defaultId = "2edcff78-f389-4f35-a27d-a1a86c905269",
            bool admin = false) 
            => new AccountDto { 
                Id = Guid.NewGuid(), 
                Name = name, 
                LastName = lastName, 
                Email = email, 
                DefaultId = Guid.Parse(defaultId),
                Admin = admin
        };
    }
}
