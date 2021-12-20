using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CartItem;
using Models.Order;
using Models.Product;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.CartItem;
using ViewModels.Order;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMainRepository<OrderEntity> OrderRepo;
        IMainRepository<CartItemEntity> CartITemRepo;
        IMainRepository<ProductEntity> ProducRepo;
        Result Result;
        public OrderController(IMainRepository<OrderEntity> orderREpo,
            IMainRepository<CartItemEntity> cartItemRepo,
            IMainRepository<ProductEntity> prodcutRepo)
        {
            OrderRepo = orderREpo;
            CartITemRepo = cartItemRepo;
            ProducRepo = prodcutRepo;
            Result = new Result();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await OrderRepo.Get();
                if (orders.Count() == 0)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "There Is No Order";
                }
                else
                {
                    Result.IsSuccess = true;
                    List<GetOrderViewModel> ordersData = new List<GetOrderViewModel>();
                    foreach (var order in orders.ToList())
                    {
                        var items = await CartITemRepo.Get();
                        var itemsVM = items.Where(i => i.OrderID == order.ID).ToList()
                            .Select(i =>
                            {
                                var pdr = ProducRepo.Get(i.ProductID).Result;
                                return i.ToViewModel(pdr.Price,pdr.Discount);
                            }).ToList();
                        ordersData.Add(order.ToViewModel(itemsVM));
                    }
                    Result.Data = ordersData;
                    Result.Message = "Success";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }

        }
        [HttpGet("Status")]
        public async Task<IActionResult> GetOrderSent([FromQuery] OrderStatus status)
        {
            var orders = (await OrderRepo.Get()).Where(i => i.Status == status);
            if (orders.Count() == 0)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There Is No Order Sent Yet !";
            }
            else
            {
                Result.IsSuccess = true;
                List<GetOrderViewModel> ordersData = new List<GetOrderViewModel>();
                foreach (var order in orders.ToList())
                {
                    var items = await CartITemRepo.Get();
                    var itemsVM = items.Where(i => i.OrderID == order.ID).ToList()
                        .Select(i =>
                        {
                            var pdr = ProducRepo.Get(i.ProductID).Result;
                            return i.ToViewModel(pdr.Price,pdr.Discount);
                        }).ToList();
                    ordersData.Add(order.ToViewModel(itemsVM));
                }
                Result.Data = ordersData;
                Result.Message = "Success";
            }
            return Ok(Result);
        }
        [HttpGet("Customer/{id}")]
        public async Task<IActionResult> GetOrderSent(string id)
        {
            var orders = (await OrderRepo.Get()).Where(i => i.CustomerID == id);
            if (orders.Count() == 0)
            {
                Result.IsSuccess = false;
                Result.Data = "";
                Result.Message = "There Is No Order Has been Made By This Customer !";
            }
            else
            {
                Result.IsSuccess = true;
                List<GetOrderViewModel> ordersData = new List<GetOrderViewModel>();
                foreach (var order in orders.ToList())
                {
                    var items = await CartITemRepo.Get();
                    var itemsVM = items.Where(i => i.OrderID == order.ID).ToList()
                        .Select(i =>
                        {
                            var pdr = ProducRepo.Get(i.ProductID).Result;
                            return i.ToViewModel(pdr.Price, pdr.Discount);
                        }).ToList();
                    ordersData.Add(order.ToViewModel(itemsVM));
                }
                Result.Data = ordersData;
                Result.Message = "Success";
            }
            return Ok(Result);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(AddOrderViewModel OrderVM)
        {
            try
            {
                var order = await OrderRepo.Add(OrderVM.ToOrderEntity());
                List<CartItemEntity> Items = new List<CartItemEntity>();
                foreach (var item in OrderVM.Items)
                {
                    var product = await ProducRepo.Get(item.ProductID);
                    if (item.Amount > product.Quantity)
                    {
                        Result.IsSuccess = false;
                        Result.Data = "";
                        Result.Message = "This Amout Is Not Available";
                        return Ok(Result);
                    }

                    var cartItem = item.ToCartItemEntity();
                    cartItem.OrderID = order.ID;
                    Items.Add(cartItem);
                }
                var items = await CartITemRepo.AddRange(Items);
                if (items.Count() == 0)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Failed To Add";
                }

                if (Result.IsSuccess)
                {
                    var itemsVMT = items.Select(async i =>
                    {
                        var PDR = await ProducRepo.Get(i.ProductID);
                        return i.ToViewModel(PDR.Price, PDR.Discount);
                    }).ToList();
                    var itemsVM = itemsVMT.Select(i => i.Result);
                    Items.Clear();
                    order.TotalPrice = itemsVM.Sum(i => i.Price);
                    order = await OrderRepo.Update(order);
                    Result.IsSuccess = true;
                    Result.Data = order.ToViewModel(itemsVM.ToList());
                    Result.Message = "Success";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
        [HttpPut]
        public async Task<IActionResult> EditOrder(EditOrderViewModel OrderVM)
        {
            try
            {
                var order = await OrderRepo.Get(OrderVM.Id);
                var orderItems = await CartITemRepo.Get();
                orderItems = orderItems.Where(i => i.OrderID == order.ID);

                foreach (var item in OrderVM.Items)
                {
                    var product = await ProducRepo.Get(item.ProductId);
                    if (item.Amount > product.Quantity)
                    {
                        Result.IsSuccess = false;
                        Result.Data = "";
                        Result.Message = "This Amount Is Not Available";
                        return Ok(Result);
                    }
                    var orderItem = await CartITemRepo.Get(item.Id);
                    if (orderItem == null)
                    {
                        // insert
                        var addItem = item.ToCartItemEntity();
                        addItem.OrderID = order.ID;
                        addItem.ProductID = item.ProductId;
                        await CartITemRepo.Add(addItem);
                    }
                    else
                    {
                        //update
                        orderItem.Amount = item.Amount;
                        orderItem.Date = item.Date;
                        await CartITemRepo.Update(orderItem);
                    }
                }

                if (orderItems.Count() > OrderVM.Items.Count())
                {
                    foreach (var item in orderItems.ToList())
                    {
                        bool isExist = OrderVM.Items.Select(i => i.Id).Contains(item.ID);
                        if (!isExist)
                        {
                            await CartITemRepo.Delete(item.ID);
                        }
                    }
                }

                order = await OrderRepo.Update(order);
                var orderItemsUpdated = await CartITemRepo.Get();

                orderItems = orderItems.Where(i => i.OrderID == order.ID);

                var itemsVMT = orderItems.Select(async i =>
                 {
                     var PDR = await ProducRepo.Get(i.ProductID);
                     return i.ToViewModel(PDR.Price, PDR.Discount);
                 }).ToList();
                var itemsVM = itemsVMT.Select(i => i.Result);
                order.TotalPrice = itemsVM.Sum(i => i.Price);
                order = await OrderRepo.Update(order);
                Result.IsSuccess = true;
                Result.Data = order.ToViewModel(itemsVM.ToList());
                Result.Message = "Success";
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
        [HttpPut("Status")]
        public async Task<IActionResult> EditOrderStatus(EditOrderStatusViewModel statusVM)
        {
            try
            {
                var order = await OrderRepo.Get(statusVM.ID);
                order.Status = statusVM.OrderStatus;

                order = await OrderRepo.Update(order);
                var orderItems = await CartITemRepo.Get();

                orderItems = orderItems.Where(i => i.OrderID == order.ID);

                var itemsVMT = orderItems.Select(async i =>
                 {
                     var PDR = await ProducRepo.Get(i.ProductID);
                     return i.ToViewModel(PDR.Price, PDR.Discount);
                 }).ToList();
                var itemsVM = itemsVMT.Select(i => i.Result);
                order.TotalPrice = itemsVM.Sum(i => i.Price);
                order = await OrderRepo.Update(order);
                Result.IsSuccess = true;
                Result.Data = order.ToViewModel(itemsVM.ToList());
                Result.Message = "Success";
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                var order = await OrderRepo.Get(id);
                if (order == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "There is No Such An Order";
                }
                else
                {
                    var items = await CartITemRepo.Get();
                    items = items.Where(i => i.OrderID == order.ID);
                    items = await CartITemRepo.Delete(items);
                    var itemsVM = items.Select(i =>
                    {
                        var pdr = ProducRepo.Get(i.ProductID).Result;
                        return i.ToViewModel(pdr.Price,pdr.Discount);
                    }).ToList();
                    order = await OrderRepo.Delete(id);
                    Result.IsSuccess = true;
                    Result.Data = order.ToViewModel(itemsVM);
                    Result.Message = "Deleted Successfull ";
                }
                return Ok(Result);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
    }
}
