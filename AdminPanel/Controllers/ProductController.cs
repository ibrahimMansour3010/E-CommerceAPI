using Microsoft.AspNetCore.Mvc;
using Models.Product;
using Models.ProductImage;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Product;

namespace AdminPanel.Controllers
{
    public class ProductController : Controller
    {
        IMainRepository<ProductEntity> ProductRepo;
        IMainRepository<ProductImageEntity> ImageRepo;
        List<ProductImageEntity> AllProdctImages;

        public ProductController(IMainRepository<ProductEntity> productRepo,
            IMainRepository<ProductImageEntity> imageRepo)
        {
            ProductRepo = productRepo;
            ImageRepo = imageRepo;
            AllProdctImages = ImageRepo.Get().Result.ToList();

        }
        public async Task<IActionResult> Index()
        {
            var Products = await ProductRepo.Get();
            var ProductsModel = Products.Select(i => i.ToUserViewModel(AllProdctImages)).ToList();
            return View(ProductsModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
