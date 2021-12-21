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
            var ordersVM = allOrders.Select(i=> {
                var customer = AppRepo.UserData(i.CustomerID).Result;
                var ViewModel = (customer.Data) as GetUserViewModel;
                string name = ViewModel.Firstname + " " + ViewModel.Lastname;
                return i.ToAdminViewModel(name);
            });
            return View(ordersVM);
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
                Items.Add(ItemModel.ToViewModel(product.Price, product.Discount, product.Name));
            };
            var orderData = order.ToViewModel(Items);
            var UserModel = await AppRepo.UserData(order.CustomerID);
            var UserData = (UserModel.Data) as GetUserViewModel;
            string Username = UserData.Firstname + " " + UserData.Lastname;
            ViewBag.Name = Username;
            return View(orderData);
        }
    }
}
