using Microsoft.AspNetCore.Http;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Customer
{
    public class SignUpViewModel
    {
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string UserRole { get; set; }

    }
    public static class AddCustomerExtensions
    {
        public static ApplicationUserEntity ToAppModel(this SignUpViewModel addCustomer)
        {
            ApplicationUserEntity customerEntity =  new ApplicationUserEntity()
            {
                Firstname = addCustomer.Firstname,
                Lastname = addCustomer.Lastname,
                Email = addCustomer.Email,
                UserName = addCustomer.Username,
                Address = addCustomer.Address,
                PhoneNumber = addCustomer.Phone,
                Gender = addCustomer.Gender,
                Image = addCustomer.Image,
                NormalizedUserName="Customer",
                
            };
            return customerEntity;
        }

    }
}
