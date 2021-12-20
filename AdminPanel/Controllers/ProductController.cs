﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        List<ProductImageEntity> AllProdctImages;
        public const string CLOUD_NAME = "dv83pikdc";
        public const string API_KEY = "523772437256571";
        public const string API_SECRIT = "60yh2CE7kUWDA8vzqiFFY03VWRQ";
        public static Cloudinary Cloudinary;
        public ProductController(IMainRepository<ProductEntity> productRepo,
            IMainRepository<ProductImageEntity> imageRepo,
            IMainRepository<CategoryEntity> catRepo)
        {
            ProductRepo = productRepo;
            ImageRepo = imageRepo;
            AllProdctImages = ImageRepo.Get().Result.ToList();
            CatRepo = catRepo;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRIT);
            Cloudinary = new Cloudinary(account);
        }
        public async Task<IActionResult> Index()
        {
            var Products = await ProductRepo.Get();
            var ProductsModel = Products.Select(i => i.ToUserViewModel(AllProdctImages)).ToList();
            return View(ProductsModel);
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

            return RedirectToAction("Index","Product");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await ProductRepo.Delete(id);
            return View();
        }
    }
}
