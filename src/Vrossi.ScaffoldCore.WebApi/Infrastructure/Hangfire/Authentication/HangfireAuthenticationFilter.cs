using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;
using System.Security.Claims;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire.Authentication
{
    internal sealed class HangfireAuthenticationFilter : JobFilterAttribute, IServerFilter, IClientFilter
    {
        private const string TenantId = "TenantId";
        private const string AuthenticationType = "Hangfire";
        private readonly ITenantStrategy _tenantStrategy;

        public HangfireAuthenticationFilter(ITenantStrategy tenantStrategy)
        {
            _tenantStrategy = tenantStrategy;
        }

        public void OnCreated(CreatedContext filterContext) { }

        public void OnCreating(CreatingContext filterContext)
        {
            filterContext.SetJobParameter(TenantId, _tenantStrategy.GetTenantId());
        }

        public void OnPerformed(PerformedContext filterContext) { }

        public void OnPerforming(PerformingContext filterContext)
        {
            var tenantId = filterContext.GetJobParameter<Guid>(TenantId);
            var principal = GetPrincipal(tenantId);
            Thread.CurrentPrincipal = new CustomClaimsPrincipal(principal, tenantId);
        }

        private static ClaimsPrincipal GetPrincipal(Guid tenantId)
        {
            var claims = new[] { new Claim(AppClaimTypes.TenantId, tenantId.ToString()) };
            var identity = new ClaimsIdentity(claims, AuthenticationType);
            return new ClaimsPrincipal(identity);
        }
    }
}
