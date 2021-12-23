using AdminPanel.Helpers;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CartItem;
using Models.Product;
using Models.ProductCategory;
using Models.ProductImage;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Product;
using ViewModels.ProductImage;

namespace AdminPanel.Controllers
{
    public class ProductController : Controller
    {
        IMainRepository<ProductEntity> ProductRepo;
        IMainRepository<ProductImageEntity> ImageRepo;
        IMainRepository<CategoryEntity> CatRepo;
        IMainRepository<CartItemEntity> ItemRepo;
        List<ProductImageEntity> AllProdctImages;
        public const string CLOUD_NAME = "dv83pikdc";
        public const string API_KEY = "523772437256571";
        public const string API_SECRIT = "60yh2CE7kUWDA8vzqiFFY03VWRQ";
        public static Cloudinary Cloudinary;
        public ProductController(IMainRepository<ProductEntity> productRepo,
            IMainRepository<ProductImageEntity> imageRepo,
            IMainRepository<CategoryEntity> catRepo,
            IMainRepository<CartItemEntity> itemRepo)
        {
            ProductRepo = productRepo;
            ImageRepo = imageRepo;
            AllProdctImages = ImageRepo.Get().Result.ToList();
            CatRepo = catRepo;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRIT);
            Cloudinary = new Cloudinary(account);

            ItemRepo = itemRepo;
        }
        public async Task<IActionResult> Index(int page=1)
        {

            var Products = await ProductRepo.Get();
            var ProductsModel = Products.Select((pdr) =>
            {
                var cat = CatRepo.Get(pdr.CategoryID).Result;
                return pdr.ToViewModelForAdmin(cat.CategoryName);
            }).ToList();
            int pageSize = 5;
            if (page < 1)
                page = 1;
            int resCount = ProductsModel.Count();
            var pager = new Pager(resCount, page, pageSize);
            var resSkip = (page - 1) * pageSize;

            var data = ProductsModel.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Cats = (await CatRepo.Get()).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel model)
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
            model.MainImage = Path.GetFullPath(path);
            // insert image into cloudinary
            var result = Cloudinary.Upload(new ImageUploadParams()
            { File = new FileDescription(model.MainImage) });
            model.MainImage = result.Url.ToString();
            // delete image from wwwroot
            System.IO.File.Delete(path);

            ProductEntity prodcutEntity = await ProductRepo.Add(model.ToModel());
            AddProductImageViewModel productImageViewModel = new AddProductImageViewModel()
            {
                ProductID = prodcutEntity.ID,
                ImageURL = model.MainImage,
            };
            ProductImageEntity productImageEntity = productImageViewModel.ToModel();
            ProductImageEntity addImageEntity = await ImageRepo.Add(productImageEntity);

            System.IO.File.Delete(path);

            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var items = await ItemRepo.Get();

            await ItemRepo.Delete(items.Where(i => i.ProductID == id));
            await ProductRepo.Delete(id);
            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public IActionResult image(string id)
        {
            ViewBag.ProductID = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> image([FromForm] AddProductImageViewModelAdmin model)
        {

            string imgURL = await saveiMG(model.imgFile);
            if (imgURL != null)
            {
                AddProductImageViewModel model1 = new AddProductImageViewModel()
                {
                    ProductID = model.productID,
                    ImageURL = imgURL
                };
                await ImageRepo.Add(model1.ToModel());
            }
            return RedirectToAction("Details", "Product",new { id=model.productID});
        }
        private async Task<string> saveiMG(IFormFile ImageFile)
        {
            string ImageURL = "";
            if (ImageFile == null)
                return null;
            var path = Path.Combine(
                         Directory.GetCurrentDirectory(), "wwwroot",
                         ImageFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }
            // save image in cloudinary
            ImageURL = Path.GetFullPath(path);
            // insert image into cloudinary
            var result = Cloudinary.Upload(new ImageUploadParams()
            { File = new FileDescription(ImageURL) });
            ImageURL = result.Url.ToString();
            // delete image from wwwroot
            System.IO.File.Delete(path);
            return ImageURL;
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await ProductRepo.Get(id);
            ViewBag.Cats = await CatRepo.Get();
            return View(product.ToUserViewModel(AllProdctImages));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(GetEditProductViewModel model)
        {
            var product = await ProductRepo.Get(model.ID);
            await ProductRepo.Update(model.ToEntityModel());
            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public async Task<IActionResult> Details (string id)
        {
            var product = await ProductRepo.Get(id);
            string categoryName = CatRepo.Find(i => i.ID == product.CategoryID).FirstOrDefault().CategoryName;
            ViewBag.CatName = categoryName;
            return View(product.ToUserViewModel(AllProdctImages));
        }
    }
}
