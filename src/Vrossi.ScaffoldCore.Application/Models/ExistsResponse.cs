using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Models
{
    public class ExistsResponse
    {
        protected ExistsResponse() { }
        public bool Exists { get; set; }
        public static ExistsResponse Create(bool exists)
        {
            return new ExistsResponse { Exists = exists };
        }
    }
}
