using Models.Cart;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Cart
{
    public class AddCart
    {
        public float TotalPrice { get; set; }
        public CartStatus Status { get; set; }

        public string CustomerID { get; set; }
        public ApplicationUserEntity CustomerEntity { get; set; }
    }
    public static class AddCartExtensions
    {
        public static CartEntity ToCartEntity(this AddCart addCart)
        {
            return new CartEntity()
            {
                CustomerID = addCart.CustomerID,
                Status = CartStatus.Cleared,
                CustomerEntity = addCart.CustomerEntity,
                TotalPrice = 0.0f,
            };
        }

    }
}
