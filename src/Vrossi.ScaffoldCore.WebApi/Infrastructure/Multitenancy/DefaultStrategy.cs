using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;
using System.Security.Claims;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Multitenancy
{
    internal class DefaultStrategy : ITenantStrategy
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultStrategy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid? GetTenantId()
        {
            var contextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            var httpContext = contextAccessor?.HttpContext;

            var tenantId = httpContext?.Request.GetTenantId();
            if (tenantId.HasValue)
                return tenantId;

            var claimsPrincipal = _serviceProvider.GetService<ClaimsPrincipal>();
            return claimsPrincipal?.AsCustom().GetTenantId();
        }
    }
}
