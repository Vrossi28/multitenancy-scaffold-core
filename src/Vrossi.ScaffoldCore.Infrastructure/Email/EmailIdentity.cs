using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    public class EmailIdentity
    {
        public string DisplayName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string SmtpServer { get;set; }
        public int SmtpPort { get; set; }
        public string[] ContactRecipients { get; set; } = Array.Empty<string>();
        public void EnsureToSend()
        {
            if (string.IsNullOrEmpty(SenderEmail))
                throw new EmailContentException("Sender email is invalid");

            if (string.IsNullOrEmpty(SenderPassword))
                throw new EmailContentException("Sender password is invalid");

            if (string.IsNullOrEmpty(SmtpServer))
                throw new EmailContentException("SMTP server is invalid");

            if (SmtpPort <= 0)
                throw new EmailContentException("SMTP server port is invalid");
        }
    }
}
