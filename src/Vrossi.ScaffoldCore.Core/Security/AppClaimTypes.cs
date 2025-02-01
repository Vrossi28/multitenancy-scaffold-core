using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Security
{
    public class AppClaimTypes
    {
        public const string Name = ClaimTypes.Name;
        public const string Email = ClaimTypes.Email;
        public const string Role = ClaimTypes.Role;
        public const string UserId = "https://mosaic.ai/identity/claims/userid";
        public const string TenantId = "https://mosaic.ai/identity/claims/tenantid";
        public const string IsAdmin = "https://mosaic.ai/identity/claims/isadmin";
        public const string DefaultTenantId = "https://mosaic.ai/identity/claims/defaulttenantid";
    }
    public class AppClaimValueTypes
    {
        public const string Email = ClaimValueTypes.Email;
        public const string String = ClaimValueTypes.String;
        public const string Boolean = ClaimValueTypes.Boolean;
    }
}
