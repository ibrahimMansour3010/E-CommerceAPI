using Microsoft.AspNetCore.Identity;
using Models.CartItem;
using Models.Order;
using Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Customer
{
    [Flags]
    public enum Gender
    {
        Male,
        Female
    }
    public class ApplicationUserEntity : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }


        public virtual IEnumerable<OrderEntity> OrderEntities { get; set; }
        public virtual IEnumerable<ProductEntity> Products { get; set; }
    }
}
