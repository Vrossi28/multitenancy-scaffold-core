using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Accounts.Models
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public AccountDto Account { get; set; }
    }
}
