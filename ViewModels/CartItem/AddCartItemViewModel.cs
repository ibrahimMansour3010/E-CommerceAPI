using Models.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CartItem
{
    public class AddCartItemViewModel
    {
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public string ProductID { get; set; }
    }
    public static class CartItemViewModelExtension
    {
        public static CartItemEntity ToCartItemEntity(this AddCartItemViewModel cartItemViewModel)
        {
            return new CartItemEntity()
            {
                Amount = cartItemViewModel.Amount,
                Date = cartItemViewModel.Date,
                ProductID = cartItemViewModel.ProductID,
            };
        }

    }
}
