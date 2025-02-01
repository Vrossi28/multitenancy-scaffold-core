using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static IMediatRBuilder AddTransactionBehavior(this IMediatRBuilder builder) => builder.AddBehavior(typeof(TransactionDbContextBehavior<,>));
        public static IMediatRBuilder AddMediator(this IServiceCollection services, Type typeOfProgram)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (typeOfProgram == null)
                throw new ArgumentNullException(nameof(typeOfProgram));

            var builder = new MediatRBuilder(services);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining(typeOfProgram);

                // Common Behaviors:

                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlerBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(MetadataBehavior<,>));
            });

            // Publishers:

            builder.Services.AddScoped<IApplicationEventPublisher, ApplicationEventPublisher>();

            return builder;
        }
        public static IMediatRBuilder AddBehavior(this IMediatRBuilder builder, Type implementationType)
        {
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), implementationType);
            return builder;
        }
    }
}
