using Models.Customer;
using Models.Order;
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
        public DateTime Date { get; set; }

        public string ProductID { get; set; }
        public string OrderID { get; set; }
        public OrderEntity OrderEntity{ get; set; }
        public virtual ProductEntity ProductEntity { get; set; }
    }
}
