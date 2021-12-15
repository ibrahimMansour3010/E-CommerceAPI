using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CartItem;

namespace ViewModels.Order
{
    public class GetOrderViewModel
    {
        public string ID { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }

        public List<GetCartItemViewModel> Items { get; set; }
    }
    public static class GetEditOrederViewModelExtensions
    {
        public static GetOrderViewModel ToViewModel(this OrderEntity order
            ,List<GetCartItemViewModel> items)
        {
            var vm = new GetOrderViewModel();

            vm.ID = order.ID;
            vm.Status = order.Status;
            vm.Items = items;
            vm.OrderDate = order.OrderDate;
            vm.TotalPrice = items.Sum(i=>i.Price);
            return vm;
        }
    }
}
