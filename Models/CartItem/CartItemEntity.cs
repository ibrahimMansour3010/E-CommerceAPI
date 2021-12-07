using Models.Cart;
using Models.Customer;
using Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CartItem
{
    public class CartItemEntity :BaseModel
    {
        public int Amount { get; set; }
        public float TotalPrice { get; set; }
        public DateTime Date { get; set; }

        public string ProductID { get; set; }
        public string CustomerID { get; set; }
        public string CartID { get; set; }
        public CartEntity CartEntity{ get; set; }
        public virtual ProductEntity ProductEntity { get; set; }
        public virtual ApplicationUserEntity CustomerEntity { get; set; }
    }
}
