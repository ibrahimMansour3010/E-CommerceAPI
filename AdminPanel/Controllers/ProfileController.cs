using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Customer;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Customer;

namespace AdminPanel.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUserEntity> UserManager;
        private readonly SignInManager<ApplicationUserEntity> SignManager;

        IAppUserRepository UserRpo;

        public const string CLOUD_NAME = "dv83pikdc";
        public const string API_KEY = "523772437256571";
        public const string API_SECRIT = "60yh2CE7kUWDA8vzqiFFY03VWRQ";
        public static Cloudinary Cloudinary;
        public ProfileController(UserManager<ApplicationUserEntity> userManager,
            SignInManager<ApplicationUserEntity> signInManager,
            IAppUserRepository userRepo)
        {

            UserManager = userManager;
            SignManager = signInManager;
            UserRpo = userRepo;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRIT);
            Cloudinary = new Cloudinary(account);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCusomerViewModel model)
        {
            var res = await UserRpo.UserData(model.Id);
            var user = await UserManager.FindByIdAsync(model.Id);
            if (model.ImageFile != null)
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
            }
            else
            {
                model.Image = user.Image;
            }
            await UserRpo.EditProfile(model);

            return RedirectToAction("Index","Profile");
        }
    }
}
