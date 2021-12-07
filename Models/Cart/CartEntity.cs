using Models.CartItem;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Cart
{
    public enum CartStatus
    {
        Saved,
        Pending,
        Cleared
    }
    public class CartEntity:BaseModel
    {
        public float TotalPrice { get; set; }
        public CartStatus Status { get; set; }

        public virtual string CustomerID { get; set; }
        public virtual ApplicationUserEntity CustomerEntity { get; set; }
        public virtual IEnumerable<CartItemEntity> CartItemEntities{ get; set; }
    }
}
