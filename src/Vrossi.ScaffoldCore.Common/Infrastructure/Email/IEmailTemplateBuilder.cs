using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Common.Infrastructure.Email
{
    public interface IEmailTemplateBuilder
    {
        IEmailContentBuilder WithDefaultTemplate(string message, string additionalMessage, string linkText, string linkUrl, string title = null);
        IEmailContentBuilder WithTemplateWithoutButton(string message, string additionalMessage, string title = null);
    }
}
