using Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ProductImage
{
    public class ProductImageEntity:BaseModel
    {
        public string ImageURL { get; set; }
        public string ProductID { get; set; }
        public virtual ProductEntity ProductEntity { get; set; }
    }
}
