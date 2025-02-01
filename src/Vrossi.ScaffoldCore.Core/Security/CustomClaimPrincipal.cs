using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Security
{
    public sealed class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public Guid? CurrentId { get; }
        public CustomClaimsPrincipal(IPrincipal principal) : base(principal) { }
        public CustomClaimsPrincipal(IPrincipal principal, Guid? currentId) : base(principal)
        {
            CurrentId = currentId;
        }
        public override bool IsInRole(string role)
        {
            if (this.IsAdmin())
                return true;

            var roles = this.GetRoles();

            return roles.Any(q => q == role);
        }
    }
}
