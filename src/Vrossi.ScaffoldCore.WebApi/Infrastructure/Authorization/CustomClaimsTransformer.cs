using Microsoft.AspNetCore.Authentication;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;
using System.Security.Claims;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authorization
{
    public sealed class CustomClaimsTransformer : IClaimsTransformation
    {
        private readonly ITenantStrategy _tenantStrategy;

        public CustomClaimsTransformer(ITenantStrategy tenantStrategy)
        {
            _tenantStrategy = tenantStrategy;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var tenantId = _tenantStrategy.GetTenantId();
            return Task.FromResult(new CustomClaimsPrincipal(principal, tenantId) as ClaimsPrincipal);
        }
    }
}
