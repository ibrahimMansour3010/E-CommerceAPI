using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Product
{
    public class AddProductImageViewModelAdmin
    {
        public string productID{ get; set; }
        public IFormFile imgFile { get; set; }
    }
}
