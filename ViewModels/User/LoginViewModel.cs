using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Customer
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
        public string UserRole { get; set; }
    }
}
