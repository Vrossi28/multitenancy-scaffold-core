using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Core.Security;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authorization
{
    public class AuthorizationTenantFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user == null || !user.Identity.IsAuthenticated || user.IsAdmin())
                return;

            var customUser = user.AsCustom();

            if (customUser.CurrentId == null)
                return;

            var tenants = customUser.GetTenants();

            if (!tenants.Contains(customUser.CurrentId.Value))
                context.Result = new ForbidResult();
        }
    }
}
