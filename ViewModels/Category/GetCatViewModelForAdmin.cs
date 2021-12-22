using Microsoft.AspNetCore.Http;
using Models.Department;
using Models.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class GetCatViewModelForAdmin
    {
        public string ID { get; set; }
        public string CategoryName { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
    public static class GetCatViewModelForAdminExtensions
    {

        public static GetCatViewModelForAdmin ToViewModelAdmin (this CategoryEntity entity,string departmentName)
        {
            var model = new GetCatViewModelForAdmin();
            model.ID = entity.ID;
            model.CategoryName = entity.CategoryName;
            model.DepartmentID = entity.DepartmentID;
            model.ImageURL = entity.ImageURL;
            model.DepartmentName = departmentName;
            return model;
        }
    }
}
