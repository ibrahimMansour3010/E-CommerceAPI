using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Cart;
using Models.CartItem;
using Models.Customer;
using Models.Product;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.CartItem;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        IMainRepository<CartItemEntity> CartItemRepo;
        IMainRepository<ApplicationUserEntity> CustomerRepo;
        IMainRepository<ProductEntity> ProductRepo;
        IMainRepository<CartEntity> CartRepo;
        Result Result;
        public CartItemController(IMainRepository<CartItemEntity> cartItemRepo,
            IMainRepository<ProductEntity> productRepo, IMainRepository<CartEntity> cartRepo,
            IMainRepository<ApplicationUserEntity> customerRepo)
        {
            CartItemRepo = cartItemRepo;
            Result = new Result();
            ProductRepo = productRepo;
            CartRepo = cartRepo;
            CustomerRepo = customerRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await CartItemRepo.Get());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var res = await CartItemRepo.Get(id);
            if (res == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "Cannot Find This CartItem";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = res;
                Result.Message = "The CartItem Has Been Retrieved Successfully";
            }
            return Ok(Result);
        }
        [HttpGet("Customer/{id}")]
        public async Task<IActionResult> GetByCustomerID(string id)
        {
            var res = await CartItemRepo.Get();
            var CutomerCartITem = res.Where(i => i.CustomerID == id);
            if (res == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "This Customer Has No Cart Items";
            }
            else
            {
                Result.IsSuccess = true;
                Result.Data = res;
                Result.Message = "The CartItem Has Been Retrieved Successfully For This Customer";
            }
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(AddCartItemViewModel cartItemViewModel)
        {
            var product = await ProductRepo.Get(cartItemViewModel.ProductID);
            int productQuantity = product.Quantity;
            if (cartItemViewModel.Amount > productQuantity)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "This Amount is Not Found";
            }
            else
            {
                var cartItem = await CartItemRepo.Add(cartItemViewModel.ToModel());
                var customer = await CustomerRepo.Get(cartItem.CustomerID);
                var cart = customer.CartEntity;
                if (cart.Status == CartStatus.Cleared)
                {
                    cart.Status = CartStatus.Pending;
                    await CartRepo.Update(cart);
                }
                product.Quantity = product.Quantity - cartItemViewModel.Amount;
                var updatedProduct = await ProductRepo.Update(product);
                if(cartItem == null || updatedProduct == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Cannot Add This Product In The Cart";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = cartItem;
                    Result.Message = "The Product Has Been Added Successfully In Your Cart";
                }
            }
            return Ok(Result);
        }
        [HttpPut]
        public async Task<IActionResult> Post(GetEditCartItemViewModel getEditCartItemViewModel)
        {
            var cartItem = await CartItemRepo.Get(getEditCartItemViewModel.ID);
            if (cartItem == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There is No Cart Item Has This ID";
            }
            else
            {
                var product = await ProductRepo.Get(getEditCartItemViewModel.ProductID);
                if (getEditCartItemViewModel.Amount > product.Quantity)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "This Amount Not Available";
                }
                else
                {
                    cartItem.Amount = getEditCartItemViewModel.Amount;
                    cartItem.Date = getEditCartItemViewModel.Date;
                    cartItem.TotalPrice = getEditCartItemViewModel.Price * getEditCartItemViewModel.Amount;
                    cartItem = await CartItemRepo.Update(cartItem);
                    Result.IsSuccess = true;
                    Result.Data = cartItem;
                    Result.Message = "The Cart Item Updated Successfully";
                }
            }
            return Ok(Result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var cartItem = await CartItemRepo.Delete(id);
            if(cartItem == null)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There Is No Cart Item in With This ID";
            }
            else
            {
                var customer = await CustomerRepo.Get(cartItem.CustomerID);
                var cart = customer.CartEntity;
                if (cart.CartItemEntities.Count() == 0)
                    cart.Status = CartStatus.Cleared;
                Result.IsSuccess = false;
                Result.Data = cartItem;
                Result.Message = "There Is No Cart Item in With This ID";
            }
            return Ok(Result);
        }
    }
}
