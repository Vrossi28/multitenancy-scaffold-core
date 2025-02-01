using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Email
{
    public class EmailContentException : Exception
    {
        public EmailContentException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
