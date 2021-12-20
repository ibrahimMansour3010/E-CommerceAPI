using Models.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CartItem
{
    public class GetCartItemViewModel
    {
        public string ID { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public string ProductID { get; set; }

    }
    public static class GetEditCartItemViewModelExtensions
    {

        public static GetCartItemViewModel ToViewModel(this CartItemEntity cartItemEntity,float price,int? discount)
        {
            var vm = new GetCartItemViewModel();
            vm.ID = cartItemEntity.ID;
            vm.Amount = cartItemEntity.Amount;
            vm.Date = cartItemEntity.Date;
            vm.ProductID = cartItemEntity.ProductID;
            vm.Price = cartItemEntity.Amount * (price - ( price*discount??0)/100) ;
            return vm;
        }
        public static CartItemEntity ToCartItemEntity(this GetCartItemViewModel model)
        {
            return new CartItemEntity()
            {
                ID = model.ID,
                Amount = model.Amount,
                Date = model.Date,
                ProductID = model.ProductID,
            };
        }
    }
}
