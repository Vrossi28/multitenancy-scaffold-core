using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Security
{
    public static class CustomClaimsPrincipalExtensions
    {
        private static readonly IEnumerable<string> Empty = Enumerable.Empty<string>();
        private static readonly Dictionary<Guid, string> EmptyDictionary = new();
        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            var isAdminClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.IsAdmin);
            bool.TryParse(isAdminClaim?.Value ?? "False", out bool isAdmin);
            return isAdmin;
        }
        private static Dictionary<Guid, string> GetRolesAsDictionary(this CustomClaimsPrincipal claimsPrincipal)
        {
            var claimRole = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.Role);

            if (claimRole == null)
                return EmptyDictionary;

            if (claimsPrincipal.CurrentId == null)
                return EmptyDictionary;

            return JsonSerializer.Deserialize<Dictionary<Guid, string>>(claimRole.Value);
        }
        public static IEnumerable<string> GetRoles(this CustomClaimsPrincipal claimsPrincipal)
        {
            var roles = claimsPrincipal.GetRolesAsDictionary();

            if (claimsPrincipal.CurrentId == null || !roles.ContainsKey(claimsPrincipal.CurrentId.Value))
                return Empty;

            return roles.Values;
        }
        public static IEnumerable<Guid> GetTenants(this CustomClaimsPrincipal claimsPrincipal) => claimsPrincipal.GetRolesAsDictionary().Keys;
        public static CustomClaimsPrincipal AsCustom(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                throw new NullReferenceException(nameof(claimsPrincipal));

            return claimsPrincipal as CustomClaimsPrincipal ?? new CustomClaimsPrincipal(claimsPrincipal);
        }
        public static Guid? GetTenantId(this CustomClaimsPrincipal principal)
        {
            if (principal == null)
                throw new NullReferenceException(nameof(principal));

            var tenantClaim = principal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.TenantId);

            tenantClaim ??= principal.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.DefaultTenantId);

            if (tenantClaim == null)
                return default;

            if(!Guid.TryParse(tenantClaim.Value, out Guid tenantId))
                return default;

            return tenantId;
        }
    }
}
