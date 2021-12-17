using Models.CartItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CartItem
{
    public class EditCartItemViewModel
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string ProductId { get; set; }
    }
    public static class EditCartItemViewModelExtensions
    {
        public static CartItemEntity ToCartItemEntity (this EditCartItemViewModel vm)
        {
            return new CartItemEntity()
            {
                Amount = vm.Amount,
                Date = vm.Date,
            };
        }
    }
}
