using Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Product
{
    public class GetProductViewModelForAdmin
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
    public static class GetProductViewModelForAdminExtensions
    {
        public static GetProductViewModelForAdmin ToViewModelForAdmin (this ProductEntity entity ,
            string CatName)
        {
            GetProductViewModelForAdmin model = new GetProductViewModelForAdmin();
            model.ID = entity.ID;
            model.Name = entity.Name;
            model.Price = entity.Price;
            model.Quantity = entity.Quantity;
            model.CategoryID = entity.CategoryID;
            model.CategoryName = CatName;
            return model;
        }
    }
}
