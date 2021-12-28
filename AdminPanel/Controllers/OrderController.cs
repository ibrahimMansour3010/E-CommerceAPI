using AdminPanel.Helpers;
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
        public async Task<IActionResult> Index(int page = 1)
        {
            var allOrders = (await OrderRepo.Get()).OrderByDescending(i=>i.OrderDate);
            var ordersVM = allOrders.Select(i =>
            {
                var customer = AppRepo.UserData(i.CustomerID).Result;
                var ViewModel = (customer.Data) as GetUserViewModel;
                string name = ViewModel.Firstname + " " + ViewModel.Lastname;
                return i.ToAdminViewModel(name);
            });
            ViewBag.OrderStatuses = new List<OrderStatus>()
            {
                OrderStatus.Pending,
                OrderStatus.Delivered,
                OrderStatus.Cancel,
            };

            int pageSize = 5;
            if (page < 1)
                page = 1;
            int resCount = ordersVM.Count();
            var pager = new Pager(resCount, page, pageSize);
            var resSkip = (page - 1) * pageSize;

            var data = ordersVM.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        public List<GetOrderViewModelForAdmin> getAllOrders()
        {
            var allOrders = OrderRepo.Get().Result;
            var ordersVM = allOrders.Select(i =>
            {
                var customer = AppRepo.UserData(i.CustomerID).Result;
                var ViewModel = (customer.Data) as GetUserViewModel;
                string name = ViewModel.Firstname + " " + ViewModel.Lastname;
                return i.ToAdminViewModel(name);
            });
            var res = ordersVM.ToList();
            return res;
        }
        [HttpGet]
        public List<GetOrderViewModelForAdmin> getOrders([FromQuery] OrderStatus Status)
        {
            var allOrders = OrderRepo.Get().Result;
            var ordersVM = allOrders.Select(i =>
            {
                var customer = AppRepo.UserData(i.CustomerID).Result;
                var ViewModel = (customer.Data) as GetUserViewModel;
                string name = ViewModel.Firstname + " " + ViewModel.Lastname;
                return i.ToAdminViewModel(name);
            });
            var res = ordersVM.Where(i => i.Status == Status).ToList();
            return res;
        }
        [HttpGet]
        public async Task<IActionResult> Details([FromQuery] string id)
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
            ViewBag.Address = UserData.Address;
            return View(orderData);
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromQuery] string id)
        {
            var order = await OrderRepo.Get(id);
            EditOrderStatusViewModel model = new EditOrderStatusViewModel()
            {
                ID = id,
                OrderStatus = order.Status
            };
            ViewBag.orderID = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditOrderStatusViewModel model)
        {
            var order = await OrderRepo.Get(model.ID);
            if (model.OrderStatus == OrderStatus.Delivered)
            {
                var items = CartItemRepo.Find(i => i.OrderID == order.ID);
                List<ProductEntity> ProductsList = new List<ProductEntity>();
                foreach (var item in items)
                {
                    var product = await ProductRepo.Get(item.ProductID);
                    if (item.Amount <= product.Quantity)
                    {
                        product.Quantity -= item.Amount;
                        ProductsList.Add(product);
                    }
                    else
                    {
                        order.Status = OrderStatus.Cancel;
                        break;
                    }
                    order.Status = OrderStatus.Delivered;
                }
                if (order.Status == OrderStatus.Delivered)
                {
                    await ProductRepo.Update(ProductsList);
                }
                order = await OrderRepo.Update(order);
            }
            else if (model.OrderStatus == OrderStatus.Cancel)
            {
                if (order.Status == OrderStatus.Delivered)
                {
                    var items = CartItemRepo.Find(i => i.OrderID == order.ID);
                    List<ProductEntity> ProductsList = new List<ProductEntity>();
                    foreach (var item in items)
                    {
                        var product = await ProductRepo.Get(item.ProductID);

                        product.Quantity += item.Amount;
                        ProductsList.Add(product);

                        order.Status = OrderStatus.Cancel;
                    }
                    if (order.Status == OrderStatus.Cancel)
                    {
                        await ProductRepo.Update(ProductsList);
                    }
                    order = await OrderRepo.Update(order);
                }
                else
                {
                    order.Status = model.OrderStatus;
                    order = await OrderRepo.Update(order);
                }
                return RedirectToAction("Index", "Order");

            }
            else
            {
                order.Status = model.OrderStatus;
                order = await OrderRepo.Update(order);
            }

            return RedirectToAction("Index", "Order");
        }
        [HttpGet]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await OrderRepo.Delete(id);
            return RedirectToAction("Index", "Order");
        }
        [HttpGet]
        public List<GetOrderViewModelForAdmin> Customer([FromQuery] string Name)
        {
            var users = AppRepo.GetAllUsers(i => i.Firstname.Contains(Name) || i.Lastname.Contains(Name));
            List<OrderEntity> ListOfUsers = new List<OrderEntity>();
            foreach (var item in users)
            {
                var orders = OrderRepo.Find(i => i.CustomerID == item.Id);
                foreach (var order in orders)
                {
                    ListOfUsers.Add(order);
                }
            }
            var data = ListOfUsers.Select(order => {
                var user = AppRepo.GetAllUsers(u => u.Id == order.CustomerID).FirstOrDefault();

                return order.ToAdminViewModel(user.Firstname + " " + user.Lastname);
            }).ToList();
            return data;
        }
    }
}
