namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Multitenancy
{
    public static class HttpContextExtensions
    {
        private const string X_TENANT_HEADER = "X-Tenant";

        public static Guid? GetTenantId(this HttpRequest request)
        {
            if (request == null || !request.Headers.ContainsKey(X_TENANT_HEADER))
                return default;

            var tenantHeader = request.Headers[X_TENANT_HEADER];

            if (!Guid.TryParse(tenantHeader, out Guid tenantId))
                return default;

            return tenantId;
        }
    }
}
