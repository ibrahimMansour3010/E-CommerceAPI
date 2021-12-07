using Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Cart
{
    public class GetEditCartViewModel
    {
        public string ID { get; set; }
        public float TotalPrice { get; set; }
        public CartStatus Status { get; set; }
    }
    public static class GetEditCartViewModelExtensions
    {
        public static GetEditCartViewModel ToViewModel(this CartEntity cartEntity)
        {
            return new GetEditCartViewModel()
            {
                ID = cartEntity.ID,
                Status = cartEntity.Status,
                TotalPrice = cartEntity.TotalPrice
            };
        }
    }
}
