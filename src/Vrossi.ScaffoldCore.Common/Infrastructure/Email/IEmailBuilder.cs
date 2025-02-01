using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Email
{
    public interface IEmailBuilder
    {
        IEmailTemplateBuilder Create(string subject, List<string> destinations);
        IEmailTemplateBuilder Create(string subject, string destination);
    }
}
