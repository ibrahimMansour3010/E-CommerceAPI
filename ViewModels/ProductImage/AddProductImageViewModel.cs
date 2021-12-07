using Models.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProductImage
{
    public class AddProductImageViewModel
    {
        public string ImageURL { get; set; }
        public string ProductID { get; set; }
    }
    public static class ProductImageViewModelExtensions
    {
        public static ProductImageEntity ToModel(this AddProductImageViewModel productImageViewModel)
        {
            return new ProductImageEntity()
            {
                ImageURL = productImageViewModel.ImageURL,
                ProductID = productImageViewModel.ProductID,
            };
        }
    }
}
