using Microsoft.AspNetCore.Http;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.User
{
    public class GetUserViewModel
    {
        public string Id { get; set; }
        public IFormFile ImageFile { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string UserRole { get; set; }

    }
    public static class GetUserViewModelExtension
    {
        public static GetUserViewModel ToViewModel (this ApplicationUserEntity applicationUserEntity,string role)
        {
            return new GetUserViewModel()
            {
                Id = applicationUserEntity.Id,
                Firstname = applicationUserEntity.Firstname,
                Lastname = applicationUserEntity.Lastname,
                Email = applicationUserEntity.Email,
                Address = applicationUserEntity.Address,
                Phone = applicationUserEntity.PhoneNumber,
                Username = applicationUserEntity.UserName,
                Gender = applicationUserEntity.Gender,
                Image = applicationUserEntity.Image,
                UserRole = role
            };
        }
    }
}
