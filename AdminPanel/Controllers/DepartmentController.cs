using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Department;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Department;

namespace AdminPanel.Controllers
{
    public class DepartmentController : Controller
    {
        IMainRepository<DepartmentEntity> DeptRep;
        public DepartmentController(IMainRepository<DepartmentEntity> deptRep)
        {
            DeptRep = deptRep;
        }
        public async Task<IActionResult> Index()
        {
            var depts = await DeptRep.Get();
            return View(depts);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(AddDepartmentViewModel model)
        {
            try
            {
                var dept = await DeptRep.Add(model.ToDeprtmentModel());
                return RedirectToAction("Index", "Department");
            }
            catch (Exception)
            {

                return View();
            }

        }
        [HttpGet]
        public IActionResult Edit([FromQuery] string id)
        {
            ViewBag.DeptID = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] GetEditDepartmentViewModel model)
        {
            try
            {

                var dept = await DeptRep.Get(model.ID);
                dept.DepartmentName = model.DepartmentName;
                dept = await DeptRep.Update(dept);
                return RedirectToAction("Index", "Department");
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            try
            {

                await DeptRep.Delete(id);
                return RedirectToAction("Index", "Department");
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
