using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Core.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = claimsPrincipal.FindFirst(AppClaimTypes.UserId);

            if (userIdClaim == null)
                throw new Exception($"{nameof(AppClaimTypes.UserId)} not found at ClaimsPrincipal");

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                throw new Exception("Failure when converting ID");

            return userId;
        }
    }
}
