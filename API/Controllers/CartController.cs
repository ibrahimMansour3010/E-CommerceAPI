using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Cart;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Cart;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CartController : ControllerBase
    {
        IMainRepository<CartEntity> CartRepo;
        Result Result;
        public CartController(IMainRepository<CartEntity> cartRepo)
        {
            CartRepo = cartRepo;
            Result = new Result();
        }

        [HttpPut]
        public async Task<IActionResult> editAsync(GetEditCartViewModel getEditCartViewModel)
        {
            try
            {
                var res = await CartRepo.Update(getEditCartViewModel.ToModel());
                if(res != null)
                {
                    Result.IsSuccess = true;
                    Result.Data = res.ToViewModel();
                    Result.Message = "Success";
                }
                else
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Not Updated";
                }

            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
            }
            return Ok(Result);
        }
    }
}
