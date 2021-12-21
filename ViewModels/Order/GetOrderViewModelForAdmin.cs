using Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CartItem;

namespace ViewModels.Order
{
    public class GetOrderViewModelForAdmin
    {
        public string ID { get; set; }
        public float TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerID{ get; set; }
        public string CustomerName{ get; set; }
        public List<GetCartItemViewModel> Items { get; set; }
    }
    public static class GetOrderViewModelForAdminExtensions
    {
        public static GetOrderViewModelForAdmin ToAdminViewModel(this OrderEntity entity,string name)
        {
            var model = new GetOrderViewModelForAdmin();
            model.ID = entity.ID;
            model.OrderDate = entity.OrderDate;
            model.Status = entity.Status;
            model.TotalPrice = entity.TotalPrice;
            model.CustomerID = entity.CustomerID;
            model.CustomerName = name ;
            return model;
        }
    }
}
