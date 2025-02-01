using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    internal class MediatRBuilder : IMediatRBuilder
    {
        public MediatRBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }

    public interface IMediatRBuilder
    {
        IServiceCollection Services { get; }
    }
}
