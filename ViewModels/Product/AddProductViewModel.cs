using Microsoft.AspNetCore.Http;
using Models.Product;
using Models.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Product
{
    public class AddProductViewModel
    {
        public string Name { get; set; }
        public int? Discount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string MainImage { get; set; }
        public IFormFile ImageFile { get; set; }
        public string CategoryID { get; set; }
        public string AdmainID { get; set; }
        public List<string>  Images { get; set; }
        public string ImageURL { get; set; }
    }
    public static class ProductViewModelExtensions
    {
        public static  ProductEntity ToModel(this AddProductViewModel productViewModel)
        {
            return new ProductEntity()
            {
                Name = productViewModel.Name,
                Price = productViewModel.Price,
                Quantity = productViewModel.Quantity,
                Discount = productViewModel.Discount,
                Description = productViewModel.Description,
                CategoryID = productViewModel.CategoryID,
                AdmainID = productViewModel.AdmainID,
            };
        }
    }
}
