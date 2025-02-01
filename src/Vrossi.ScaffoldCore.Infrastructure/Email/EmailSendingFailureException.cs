using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    public class EmailSendingFailureException : Exception
    {
        public EmailSendingFailureException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
