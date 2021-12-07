using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ProductImage;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.ProductImage;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        IMainRepository<ProductImageEntity> PdrImgRepo;
        Result Result;
        public ProductImageController(IMainRepository<ProductImageEntity> pdrImgRepo)
        {
            PdrImgRepo = pdrImgRepo;
            Result = new Result();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok((await PdrImgRepo.Get()).Select(i=>i.ToViewModel()));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]string id)
        {
            var res = await PdrImgRepo.Get(id);
            if (res == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "Cannot Find This Image";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = res.ToViewModel();
                Result.Message = "The Image Has Been Retrieved Successfully";
            }
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddProductImageViewModel productImageViewModel)
        {
            var res = await PdrImgRepo.Add(productImageViewModel.ToModel());
            if (res == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "Cannot Add This Image";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = res.ToViewModel();
                Result.Message = "The Image Has Been Added Successfully";
            }
           
            return Ok(Result);
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody]GetEditProductImageViewModel getEditProductImageViewModel)
        {
            var productImage = await PdrImgRepo.Get(getEditProductImageViewModel.ID);
            if (productImage == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "Cannot Find This Image";
            }
            else
            {
                productImage.ProductID = getEditProductImageViewModel.ProductID;
                productImage.ImageURL = getEditProductImageViewModel.ImageURL;
                productImage = await PdrImgRepo.Update(productImage);
                Result.IsSuccess = true;
                Result.Data = productImage;
                Result.Message = "The Image Has Been Updated Successfully";
            }
           
            return Ok(Result);
        }
    }
}
