using AdminPanel.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public const string CLOUD_NAME = "dv83pikdc";
        public const string API_KEY = "523772437256571";
        public const string API_SECRIT = "60yh2CE7kUWDA8vzqiFFY03VWRQ";
        public static Cloudinary Cloudinary;

        public CategoryController(IMainRepository<CategoryEntity> catRepo,
            IMainRepository<DepartmentEntity> deptRepo,
            IWebHostEnvironment webHostEnvironment)
        {
            CatRepo = catRepo;
            DeptRepo = deptRepo;
            HostEnvironment = webHostEnvironment;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRIT);
            Cloudinary = new Cloudinary(account);

        }
        public async Task<IActionResult> Index(int page= 1)
        {
            var cats = await CatRepo.Get();
            var ctsVM = cats.Select((cat) =>
            {
                var dept = DeptRepo.Get(cat.DepartmentID).Result;
                return cat.ToViewModelAdmin(dept.DepartmentName);
            });
            
            int pageSize = 3;
            if (page < 1)
                page = 1;
            int resCount = ctsVM.Count();
            var pager = new Pager(resCount, page, pageSize);
            var resSkip = (page - 1) * pageSize;

            var data = ctsVM.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var depts = await DeptRepo.Get();
            ViewBag.Depts = depts.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                var path = Path.Combine(
                  Directory.GetCurrentDirectory(), "wwwroot",
                  model.ImageFile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                model.ImageURL = Path.GetFullPath(path);


                // insert image into cloudinary
                var result = Cloudinary.Upload(new ImageUploadParams()
                { File = new FileDescription(model.ImageURL) });
                model.ImageURL = result.Url.ToString();

                System.IO.File.Delete(path);

                // insert into database
                await CatRepo.Add(model.ToCategoryModel());
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var cat = await CatRepo.Get(id);
            var publicID = cat.ImageURL.Split('/')[cat.ImageURL.Split('/').Length - 1];
            publicID = publicID.Substring(0, publicID.IndexOf('.'));

            Cloudinary.Destroy(new DeletionParams(publicID));
            await CatRepo.Delete(id);
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] string id)
        {
            ViewBag.Depts = (await DeptRepo.Get()).ToList();
            var cat = await CatRepo.Get(id);
            var model = cat.ToViewModel();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] GetEditCategoryViewModel model)
        {
            var enity = await CatRepo.Get(model.ID);
            if (model.ImageFile != null)
            {
                // save image in wwwroot
                var path = Path.Combine(
                         Directory.GetCurrentDirectory(), "wwwroot",
                         model.ImageFile.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                // save image in cloudinary
                model.ImageURL = Path.GetFullPath(path);
                // insert image into cloudinary
                var result = Cloudinary.Upload(new ImageUploadParams()
                { File = new FileDescription(model.ImageURL) });
                model.ImageURL = result.Url.ToString();
                // delete image from wwwroot
                System.IO.File.Delete(path);
            }
            else
            {
                model.ImageURL = enity.ImageURL;
            }
            // updaate in db
            enity = model.ToEntityModel();
            await CatRepo.Update(enity);

            return RedirectToAction("Index", "Category");
        }
    }
}
