using AdminPanel.Helpers;
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
        public async Task<IActionResult> Index(int page = 1)
        {
            var depts = await DeptRep.Get();
            int pageSize = 5;
            if (page < 1)
                page = 1;
            int resCount = depts.Count();
            var pager = new Pager(resCount, page, pageSize);
            var resSkip = (page - 1) * pageSize;

            var data = depts.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
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
        public async Task<IActionResult> Edit([FromQuery] string id)
        {
            ViewBag.DeptID = id;
            var dept = await DeptRep.Get(id);
            return View(dept.ToDeptViewModel());
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
