using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        public async Task<IActionResult> Add([FromForm] AddCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {

                model.ImageURL = Path.GetFullPath(model.ImageFile.FileName);
                    

                // insert image into cloudinary
                var result =  Cloudinary.Upload(new ImageUploadParams() 
                { File = new FileDescription(model.ImageURL) });
                model.ImageURL = result.Url.ToString();
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
            var publicID = cat.ImageURL.Split('/')[cat.ImageURL.Split('/').Length-1];
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

                var newPath = Path.Combine(HostEnvironment.WebRootPath, "Image", model.ImageFile.FileName);
                if (!System.IO.File.Exists(newPath))
                {
                    // remove old file from wwwroor
                    var oldPath = Path.Combine(HostEnvironment.WebRootPath, "Image", enity.ImageURL);
                    System.IO.File.Delete(oldPath);
                    // add new path
                    string wwwroot = HostEnvironment.WebRootPath;
                    string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    model.ImageURL = filename = filename + DateTime.Now.ToString("hhmmssfffffff") + extension;
                    string path = Path.Combine(wwwroot + "/Image/", filename);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                }
                // updaate in db
            }
            else
            {
                model.ImageURL = enity.ImageURL;
            }
            enity = model.ToEntityModel();
            await CatRepo.Update(enity);

            return RedirectToAction("Index", "Category");
        }
    }
}
