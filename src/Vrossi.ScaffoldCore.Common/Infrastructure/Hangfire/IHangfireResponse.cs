using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire
{
    public interface IHangfireResponse
    {
        HttpStatusCode StatusCode { get; }
        string Message { get; }
    }
}
