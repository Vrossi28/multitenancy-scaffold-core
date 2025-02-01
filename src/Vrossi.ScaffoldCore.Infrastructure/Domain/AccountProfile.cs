using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Domain
{
    public class AccountProfile : AggregateRoot
    {
        protected AccountProfile() { }

        public AccountProfile(RoleType profile, Guid tenantId) : base(Guid.NewGuid())
        {
            Profile = profile;
            TenantId = tenantId;
        }

        public AccountProfile(RoleType profile, Guid tenantId, Guid accountId) : base(Guid.NewGuid())
        {
            Profile = profile;
            TenantId = tenantId;
            AccountId = accountId;
        }

        public RoleType Profile {  get; }
        public Guid TenantId { get; }
        public Guid AccountId { get; }

        public virtual Tenant Tenant { get; private set; }
    }
}
