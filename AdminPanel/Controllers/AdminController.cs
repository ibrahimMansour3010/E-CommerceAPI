using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Customer;
using Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Customer;
using ViewModels.User;

namespace AdminPanel.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUserEntity> UserManager;
        private readonly SignInManager<ApplicationUserEntity> SignManager;
        private readonly IConfiguration Config;

        public const string CLOUD_NAME = "dv83pikdc";
        public const string API_KEY = "523772437256571";
        public const string API_SECRIT = "60yh2CE7kUWDA8vzqiFFY03VWRQ";
        public static Cloudinary Cloudinary;
        public AdminController(UserManager<ApplicationUserEntity> userManager,
            SignInManager<ApplicationUserEntity> signInManager,
            IConfiguration configuration)
        {
            UserManager = userManager;
            SignManager = signInManager;

            this.Config = configuration;
            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRIT);
            Cloudinary = new Cloudinary(account);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allusers = await UserManager.GetUsersInRoleAsync("Admin");
            return View(allusers.Select(i=>i.ToViewModel()).ToList());
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Sigup([FromForm] SignUpViewModel model)
        {
            var path = Path.Combine(
                 Directory.GetCurrentDirectory(), "wwwroot",
                 model.ImageFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }
            model.Image = Path.GetFullPath(path);

            // insert image into cloudinary
            var result = Cloudinary.Upload(new ImageUploadParams()
            { File = new FileDescription(model.Image) });
            model.Image = result.Url.ToString();

            System.IO.File.Delete(path);

            ApplicationUserEntity user = model.ToAppModel();
            var res = await UserManager.CreateAsync(user, model.Password);

            if (res.Succeeded)
            {
                var appUser = await UserManager.FindByNameAsync(user.UserName);
                res = await UserManager.AddToRoleAsync(appUser, model.UserRole);

                if (res.Succeeded)
                {

                    return RedirectToAction("Login", "Admin");

                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // get user who has the login view model
            var user = await UserManager.FindByNameAsync(model.Username);
            var result =
                await SignManager.PasswordSignInAsync(model.Username
                , model.Password, model.IsRemembered, true);
            if (result.Succeeded && await UserManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await UserManager.GetRolesAsync(user);
                if (roles.Contains(model.UserRole))
                {
                    // create claims 
                    var claims = new List<Claim>(){
                new Claim("UserID" , user.Id),
                new Claim(ClaimTypes.Role , model.UserRole),
                };
                    // create key
                    var key =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["JWT:Secret"]));
                    // create token
                    var Token = new JwtSecurityToken
                        (
                            issuer: Config["JWT:ValidIssuer"],
                            audience: Config["JWT:ValidAudiene"],
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials:
                            new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                            claims: claims
                        );

                    // return token
                    string token = new JwtSecurityTokenHandler().WriteToken(Token);
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);

                    return RedirectToAction("Index", "Department");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string Email)
        {

            var user = await UserManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                return RedirectToAction("ResetPassword", "Admin", new { Email = Email, Token = token });
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string Email,
            string Token)
        {
            if (Token != null && Email != null)
            {
                var model = new ResetPasswordViewModel()
                {
                    Email = Email,
                    Token = Token
                };
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if(user != null)
            {
                var res = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (res.Succeeded)
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View();
        }
    }
}
