using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Swagger.Filters
{
    public class XTenantOperationFilter : IOperationFilter
    {
        private static readonly Guid TENANT_EXAMPLE = Guid.NewGuid();
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            var attributes = apiDescription.CustomAttributes();
            var filters = apiDescription.ActionDescriptor.FilterDescriptors.Select(filterInfo => filterInfo.Filter);
            var isAuthorized = attributes.Any(attr => attr is AuthorizeAttribute) || filters.Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = attributes.Any(attr => attr is AllowAnonymousAttribute) || filters.Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Tenant",
                    In = ParameterLocation.Header,
                    Description = $"Client's tenant, ex: {TENANT_EXAMPLE}",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "uuid"
                    }
                });
            }
        }
    }
}
