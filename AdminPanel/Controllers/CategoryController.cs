using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models.Department;
using Models.ProductCategory;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Category;

namespace AdminPanel.Controllers
{
    public class CategoryController : Controller
    {
        IMainRepository<CategoryEntity> CatRepo;
        IMainRepository<DepartmentEntity> DeptRepo;
        IWebHostEnvironment HostEnvironment;
        public CategoryController(IMainRepository<CategoryEntity> catRepo,
            IMainRepository<DepartmentEntity> deptRepo,
            IWebHostEnvironment webHostEnvironment)
        {
            CatRepo = catRepo;
            DeptRepo = deptRepo;
            HostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var cats = await CatRepo.Get();
            return View(cats);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var depts = await DeptRepo.Get();
            ViewBag.Depts = depts.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                // save image into wwwroot/image
                string wwwroot = HostEnvironment.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                model.ImageURL = filename = filename + DateTime.Now.ToString("hhmmssfffffff")+extension;
                string path = Path.Combine(wwwroot + "/Image/", filename);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
                // insert into database
                await CatRepo.Add(model.ToCategoryModel());
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery]string id )
        {
            var cat = await CatRepo.Get(id);
            var imagePath = Path.Combine(HostEnvironment.WebRootPath, "Image", cat.ImageURL);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            await CatRepo.Delete(id);
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
