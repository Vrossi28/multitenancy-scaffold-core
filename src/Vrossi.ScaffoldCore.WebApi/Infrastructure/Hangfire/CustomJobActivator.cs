using Hangfire;
using System.Diagnostics.CodeAnalysis;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire
{
    internal sealed class CustomJobActivator : JobActivator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomJobActivator([NotNull] IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new CustomJobActivatorScope(CustomJobActivatorScope.Current ?? _serviceScopeFactory.CreateScope());
        }
    }
}
