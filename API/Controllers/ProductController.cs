using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Product;
using Models.ProductImage;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Product;
using ViewModels.ProductImage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IMainRepository<ProductEntity> ProductRepo;
        IMainRepository<ProductImageEntity> ProductImageRepo;
        List<ProductImageEntity> AllProdctImages;
        Result Result;
        public ProductController(IMainRepository<ProductEntity> productRepo,
            IMainRepository<ProductImageEntity> productImageRepo)
        {
            ProductRepo = productRepo;
            ProductImageRepo = productImageRepo;
            Result = new Result();
            AllProdctImages = productImageRepo.Get().Result.ToList();
        }
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var allProducts = (await ProductRepo.Get());
            var allProductModels = allProducts.Select(i => i.ToUserViewModel(AllProdctImages));
            Result.Data = allProductModels;
            return Ok(Result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await ProductRepo.Get(id);
            if(product == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There is no Product With This ID";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = product.ToUserViewModel(AllProdctImages);
                Result.Message = "There is no Product With This ID";

            }
            return Ok(Result);
        }
        [HttpGet("Category/{CategoryID}")]
        public async Task<IActionResult> GetProductsByCatID(string CategoryID)
        {
            var allProducts = (await ProductRepo.Get()).Where(i => i.CategoryID == CategoryID);
            if(allProducts == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There is No Category With This iD";
            }
            else
            {
                var allproductUserModel = allProducts.Select(i => i.ToUserViewModel(AllProdctImages));
                Result.Data = allproductUserModel;
                Result.IsSuccess = true;
                Result.Data = allproductUserModel;
                Result.Message = "All Product In This Category Have Been Retrieved Successfully";
            }
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddProductViewModel productViewModel)
        {
            ProductEntity prodcutEntity = await ProductRepo.Add(productViewModel.ToModel());
            AddProductImageViewModel productImageViewModel = new AddProductImageViewModel()
            {
                ProductID = prodcutEntity.ID,
                ImageURL = productViewModel.MainImage,
            };
            ProductImageEntity productImageEntity = productImageViewModel.ToModel();
            ProductImageEntity addImageEntity = await ProductImageRepo.Add(productImageEntity);
            Result.Data = prodcutEntity.ToUserViewModel(AllProdctImages);
            return Ok(Result);
        }
        [HttpPut]
        public async Task<IActionResult> Edit(GetEditProductViewModel getEditProductViewModel)
        {
            ProductEntity prodcutEntity = await ProductRepo.Get(getEditProductViewModel.ID);
            if(prodcutEntity == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There Is No Product Has This ID";
            }
            else
            {
                prodcutEntity.Name = getEditProductViewModel.Name;
                prodcutEntity.Price = getEditProductViewModel.Price;
                prodcutEntity.Quantity = getEditProductViewModel.Quantity;
                prodcutEntity.Description = getEditProductViewModel.Description;
                prodcutEntity.Discount = getEditProductViewModel.Discount;
                prodcutEntity.CategoryID = getEditProductViewModel.CategoryID;
                prodcutEntity =  await ProductRepo.Update(prodcutEntity);
                Result.IsSuccess = true;
                Result.Data = prodcutEntity;
                Result.Message = "This Product Has Been updated Successfully";
            }
            return Ok(Result);
        }
    }
}
