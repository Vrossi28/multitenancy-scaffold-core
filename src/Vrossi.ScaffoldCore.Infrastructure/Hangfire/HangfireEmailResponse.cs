using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Hangfire
{
    public class HangfireEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
