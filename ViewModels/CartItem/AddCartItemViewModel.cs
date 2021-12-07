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
        public float Price { get; set; }
        public DateTime Date { get; set; }

        public string ProductID { get; set; }
        public string CustomerID { get; set; }
    }
    public static class CartItemViewModelExtension
    {
        public static CartItemEntity ToModel(this AddCartItemViewModel cartItemViewModel)
        {
            return new CartItemEntity()
            {
                Amount = cartItemViewModel.Amount,
                Date = cartItemViewModel.Date,
                TotalPrice = cartItemViewModel.Price * cartItemViewModel.Amount,
                CustomerID = cartItemViewModel.CustomerID,
                ProductID = cartItemViewModel.ProductID
            };
        }
    }
}
