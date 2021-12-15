using Models.CartItem;
using Models.Customer;
using Models.ProductCategory;
using Models.ProductImage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Product
{
    public class ProductEntity:BaseModel
    {
        public string Name { get; set; }
        public int? Discount { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

        public string CategoryID { get; set; }
        public string AdmainID { get; set; }
        public virtual ApplicationUserEntity ApplicationUser { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual IEnumerable<CartItemEntity> CartItemEntities { get; set; }
        public virtual IEnumerable<ProductImageEntity> ProductImageEntities { get; set; }

    }
}
