using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication
{
    public class JwtTokenOptions
    {
        public string SecretKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public double ExpiryHours { get; set; } = 24;
    }
}
