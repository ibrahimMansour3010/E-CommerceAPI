﻿using Microsoft.AspNetCore.Http;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Customer
{
    public class EditCusomerViewModel
    {
        public string Id { get; set; }
        public IFormFile ImageFile { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string UserRole { get; set; }

    }
}
