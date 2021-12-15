using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CartItem;

namespace ViewModels.Order
{
    public class EditOrderViewModel
    {
        public string Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        public List<EditCartItemViewModel> Items { get; set; }
    }
    public static class EditOrderViewModelExtensions
    {
        public static OrderEntity ToOrderEntity (this EditOrderViewModel orderVM)
        {
            return new OrderEntity()
            {
                ID = orderVM.Id,
                Status = orderVM.Status,
                OrderDate = orderVM.OrderDate
            };
        }
    }
}
