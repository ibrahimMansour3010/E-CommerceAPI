using Models.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Department
{
    public class AddDepartmentViewModel
    {
        public string DepartmentName { get; set; }
    }

    public static class DepartmentViewModelExtensions
    {
        public static DepartmentEntity ToDeprtmentModel(this AddDepartmentViewModel departmentViewModel)
        {
            return new DepartmentEntity()
            {
                DepartmentName = departmentViewModel.DepartmentName
            };
        }
    }
}
