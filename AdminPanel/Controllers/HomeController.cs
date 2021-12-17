using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModels.Customer;

namespace AdminPanel.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Result Result;
        private readonly IAppUserRepository AppUserRepository;
        public HomeController(ILogger<HomeController> logger,
            IAppUserRepository _appUserRepository)
        {
            _logger = logger;
            AppUserRepository = _appUserRepository;
            Result = new Result();
        }
        [HttpGet]
      //  [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.UserRole = "Admin";
                Result = await AppUserRepository.Login(loginViewModel);
                if (Result.IsSuccess)
                {

                   //HttpContext.Session.SetString("JWToken", Result.Data.ToString());

                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return View();
            }
        }
        //[Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            AppUserRepository.Logout();
            return RedirectToAction("Login", "Home");
        }
        //[Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        //[Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
