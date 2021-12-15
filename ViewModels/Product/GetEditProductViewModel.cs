using Models.Product;
using Models.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Product
{
    public class GetEditProductViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int? Discount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string CategoryID { get; set; }
        public List<string> Images { get; set; }
    }
    public static class ProductViewModelForUserExtensions
    {
        public static GetEditProductViewModel ToUserViewModel(this ProductEntity prodcutEntity, List<ProductImageEntity> productImageEntities)
        {
            GetEditProductViewModel usernm = new GetEditProductViewModel();
            usernm.ID = prodcutEntity.ID;
            usernm.Name = prodcutEntity.Name;
            usernm.Price = prodcutEntity.Price;
            usernm.Quantity = prodcutEntity.Quantity;
            usernm.Description = prodcutEntity.Description;
            usernm.Discount = prodcutEntity.Discount;
            usernm.Images = productImageEntities.Where(img => img.ProductID == prodcutEntity.ID)
                .Select(img => img.ImageURL).ToList();
            usernm.CategoryID = prodcutEntity.CategoryID;
            return usernm;
            
        }
    }
}
