using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    internal class MetadataBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IMetadataRequest, IRequest<TResponse>
        where TResponse : Response
    {
        private readonly CustomClaimsPrincipal _claimsPrincipal;
        private readonly ITenantStrategy _tenantStrategy;

        public MetadataBehavior(ITenantStrategy tenantStrategy, ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal.AsCustom();
            _tenantStrategy = tenantStrategy;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_claimsPrincipal.Identity.IsAuthenticated)
            {
                request.UserId = _claimsPrincipal.GetUserId();
                request.XTenantId = _tenantStrategy.GetTenantId();
                request.IsAdmin = _claimsPrincipal.IsAdmin();

                if (!request.IsAdmin)
                {
                    request.AllowedTenantIds = _claimsPrincipal.GetTenants();
                }
            }

            return await next();
        }
    }
}
