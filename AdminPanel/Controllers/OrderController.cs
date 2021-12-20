using Microsoft.AspNetCore.Mvc;
using Models.CartItem;
using Models.Customer;
using Models.Order;
using Models.Product;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.CartItem;
using ViewModels.Order;
using ViewModels.User;

namespace AdminPanel.Controllers
{
    public class OrderController : Controller
    {
        IMainRepository<OrderEntity> OrderRepo;
        IMainRepository<CartItemEntity> CartItemRepo;
        IMainRepository<ProductEntity> ProductRepo;
        IAppUserRepository AppRepo;
        public OrderController(IMainRepository<OrderEntity> orderRepo,
            IMainRepository<CartItemEntity> cartItemRepo,
            IMainRepository<ProductEntity> productRepo,
            IAppUserRepository appRepo)
        {
            OrderRepo = orderRepo;
            CartItemRepo = cartItemRepo;
            ProductRepo = productRepo;
            AppRepo = appRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allOrders = await OrderRepo.Get();
            return View(allOrders.Select(i=>i.ToAdminViewModel()));
        }
        [HttpGet]
        public async Task<IActionResult> Details([FromQuery]string id)
        {
            var order = await OrderRepo.Get(id);
            var items = await CartItemRepo.Get();
            items = items.Where(i => i.OrderID == order.ID);
            List<GetCartItemViewModel> Items = new List<GetCartItemViewModel>();
            foreach (var ItemModel in items)
            {
                var product = await ProductRepo.Get(ItemModel.ProductID);
                //var UserModel = AppRepo.GetCustomerData(order.CustomerID)
                //                    .Result.Data;
                //var UserData = (UserModel.User) as GetUserViewModel;
                //string Username = UserData.Firstname + " " + UserData.Lastname;
                //ViewBag.Name = Username;
                Items.Add(ItemModel.ToViewModel(product.Price, product.Discount));
            };

            return View(Items);
        }
    }
}
