using Models.Department;
using Models.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class AddCategoryViewModel
    {
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentID { get; set; }
    }
    public static class CategoryViewModelExtensions
    {
        public static CategoryEntity ToCategoryModel(this AddCategoryViewModel categoryViewModel)
        {
            return new CategoryEntity()
            {
                CategoryName = categoryViewModel.CategoryName,
                ImageURL = categoryViewModel.ImageURL,
                DepartmentID = categoryViewModel.DepartmentID
            };
        }
    }
}
