using Vrossi.ScaffoldCore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Email
{
    public class EmailModelRequest
    {
        public EmailModel ModelName { get; set; }
        public List<string> Recipients { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
    }
}
