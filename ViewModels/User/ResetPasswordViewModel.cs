using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Customer
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password and Confirm Password Must Match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}
