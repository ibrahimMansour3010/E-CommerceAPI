using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CartItem
{
    public class GetEditCartItemViewModel
    {
        public string ID { get; set; }
        public int Amount { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public string ProductID { get; set; }
    }
}
