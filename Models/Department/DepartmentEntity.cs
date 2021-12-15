using Models.ProductCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Department
{
    public class DepartmentEntity:BaseModel
    {
        public string DepartmentName { get; set; }
        public virtual IEnumerable<CategoryEntity> CategoryEntities { get; set; }
    }
}
