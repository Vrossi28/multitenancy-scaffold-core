using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Core.Security;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformer>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicyNames.Everyone, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
                options.AddPolicy(AppPolicyNames.AdminsOrDirectors, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Director.ToString()) || x.User.AsCustom().IsInRole(RoleType.Administrator.ToString())).Build());
                options.AddPolicy(AppPolicyNames.AdminsOrDirectorsOrManagers, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Director.ToString()) || x.User.AsCustom().IsInRole(RoleType.Administrator.ToString()) || x.User.AsCustom().IsInRole(RoleType.Manager.ToString())).Build());
                options.AddPolicy(AppPolicyNames.Admins, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Administrator.ToString())).Build());
                options.AddPolicy(AppPolicyNames.Directors, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Director.ToString())).Build());
                options.AddPolicy(AppPolicyNames.Managers, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Manager.ToString())).Build());
                options.AddPolicy(AppPolicyNames.Observers, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.Observer.ToString())).Build());
                options.AddPolicy(AppPolicyNames.TeamMembers, new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireAssertion(x => x.User.IsAdmin() || x.User.AsCustom().IsInRole(RoleType.TeamMember.ToString())).Build());
            });
        }
        public static void AddAuthorizeTenantFilter(this MvcOptions mvcOptions) => mvcOptions.Filters.Add<AuthorizationTenantFilter>();
    }
}
