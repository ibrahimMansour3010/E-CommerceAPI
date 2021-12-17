using Microsoft.AspNetCore.Http;
using Models.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class GetEditCategoryViewModel
    {
        public string ID{ get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentID { get; set; }
        public IFormFile ImageFile { get; set; }
    }
    public static class GetEditCategoryViewModelExtensions
    {
        public static GetEditCategoryViewModel ToViewModel(this CategoryEntity categoryEntity)
        {
            GetEditCategoryViewModel vm = new GetEditCategoryViewModel();
            vm.ID = categoryEntity.ID;
            vm.CategoryName = categoryEntity.CategoryName;
            vm.ImageURL = categoryEntity.ImageURL;
            vm.DepartmentID = categoryEntity.DepartmentID;
            return vm;
        }
        public static CategoryEntity  ToEntityModel(this GetEditCategoryViewModel vm)
        {
            CategoryEntity enity = new CategoryEntity();
            enity.ID = vm.ID;
            enity.CategoryName = vm.CategoryName;
            enity.ImageURL = vm.ImageURL;
            enity.DepartmentID = vm.DepartmentID;
            return enity;
        }
    }
}
