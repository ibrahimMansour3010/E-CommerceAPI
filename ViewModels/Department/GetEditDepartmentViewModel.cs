using Models.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Department
{
    public class GetEditDepartmentViewModel
    {
        public string ID { get; set; }
        public string DepartmentName { get; set; }
    }
    public static class GetDepartmentViewModelExtensions
    {
        public static GetEditDepartmentViewModel ToDeptViewModel (this DepartmentEntity departmentEntity)
        {
            GetEditDepartmentViewModel vm = new GetEditDepartmentViewModel();
            vm.ID = departmentEntity.ID;
            vm.DepartmentName = departmentEntity.DepartmentName;
            return vm;
        }
    }
}
