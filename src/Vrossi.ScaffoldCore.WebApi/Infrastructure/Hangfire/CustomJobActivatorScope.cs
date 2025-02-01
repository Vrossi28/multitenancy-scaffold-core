using Hangfire;
using System.Diagnostics.CodeAnalysis;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Hangfire
{
    internal sealed class CustomJobActivatorScope : JobActivatorScope
    {
        public CustomJobActivatorScope([NotNull] IServiceScope serviceScope)
        {
            _current.Value = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        }

        public override object Resolve(Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(_current.Value!.ServiceProvider, type);
        }

        public override void DisposeScope()
        {
            // HACK: Keep the scope active for further usage.
        }

        private static readonly ThreadLocal<IServiceScope?> _current = new(trackAllValues: false);

        public new static IServiceScope Current => _current.Value!;

        public static void Close()
        {
            if (_current.Value != null)
            {
                _current.Value.Dispose();
                _current.Value = null;
            }
        }
    }
}
