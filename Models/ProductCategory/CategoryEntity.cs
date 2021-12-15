using Models.Department;
using Models.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ProductCategory
{
    public class CategoryEntity:BaseModel
    {
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentID { get; set; }
        public virtual DepartmentEntity Department { get; set; }
        public virtual IEnumerable<ProductEntity> Products { get; set; }
    }
}
