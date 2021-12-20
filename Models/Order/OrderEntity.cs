using Models.CartItem;
using Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Order
{
    [Flags]
    public enum OrderStatus{
        Sent,
        Pending,
        Delivered
    }
    public class OrderEntity : BaseModel
    {
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual string CustomerID { get; set; }
        public virtual ApplicationUserEntity CustomerEntity { get; set; }
        public virtual IEnumerable<CartItemEntity> CartItemEntities { get; set; }
    }
}
