using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    internal class EmailContent
    {
        public List<string> Emails { get; init; } = new List<string>();
        public List<string> CC { get; init; } = new List<string>();
        public string Text { get; private set; }
        public string Body { get; private set; }
        public string Subject { get; init; }
        public void EnsureToSend()
        {
            if (string.IsNullOrEmpty(Subject))
                throw new EmailContentException("Email subject is invalid");

            if (string.IsNullOrEmpty(Text) && string.IsNullOrEmpty(Body))
                throw new EmailContentException("Email message is invalid");

            if (Emails.Count == 0)
                throw new EmailContentException("No recipient was defined");
        }
        public void SetBody(string body) => Body = body;
        public void SetText(string text) => Text = text;
    }
}
