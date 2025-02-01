using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Email
{
    public interface IEmailContentBuilder
    {
        public Task<bool> Send();
    }
}
