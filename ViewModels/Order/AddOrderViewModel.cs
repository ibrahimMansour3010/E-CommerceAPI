using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CartItem;

namespace ViewModels.Order
{
    public class AddOrderViewModel
    {
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        public List<AddCartItemViewModel> Items { get; set; }
    }
    public static class AddOrderViewModelExtensoions
    {
        public static OrderEntity ToOrderEntity(this AddOrderViewModel orderVM)
        {
            return new OrderEntity()
            {
                Status = orderVM.Status,
                OrderDate = orderVM.OrderDate
            };
        }
    }
}
