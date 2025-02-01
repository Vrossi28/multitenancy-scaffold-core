using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Multitenancy
{
    public interface ITenantStrategy
    {
        Guid? GetTenantId();
    }
}
