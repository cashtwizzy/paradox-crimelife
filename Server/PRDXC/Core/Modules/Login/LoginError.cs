using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRDXC.Core.Modules.Login
{
    public class LoginError
    {
        public string Error { get; set; }

        public LoginError(string error)
        {
            Error = error;
        }
    }
}
//Twizzy#9904 https://github.com/cashtwizzy