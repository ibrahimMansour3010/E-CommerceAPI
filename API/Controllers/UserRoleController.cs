using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Customer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Result result;
        private readonly UserManager<ApplicationUserEntity> userManager;

        public UserRoleController(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUserEntity> userManager) {
            this.roleManager = roleManager;
            this.result = new Result();
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var roles = roleManager.Roles;
            
            if(roles == null)
            {
                result.IsSuccess = false;
                result.Data = "";
                result.Message = "Not Found";
            }
            else
            {
                result.IsSuccess = true;
                result.Data = roles.ToList();
                result.Message = "Found";
            }
            return Ok(result);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            if(role == null)
            {
                result.IsSuccess = false;
                result.Data = "";
                result.Message = "Not Found";
            }
            else
            {
                result.IsSuccess = true;
                result.Data = role;
                result.Message = "Found";
            }
            return Ok(result);
        }
        [HttpPost("{name}")]
        public async Task<IActionResult> Add(string name)
        {
            var role = new IdentityRole()
            {
                Name = name
            };
            var res = await roleManager.CreateAsync(role);
            if(res == null)
            {
                result.IsSuccess = false;
                result.Data = "";
                result.Message = "Not Added";
            }
            else
            {
                result.IsSuccess = true;
                result.Data = await roleManager.FindByNameAsync(name);
                result.Message = "Found";
            }
            return Ok(result);
        }
        [HttpDelete("{name}")]
        public async Task<IActionResult> delete(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            var res = await roleManager.DeleteAsync(role);
            if(res == null)
            {
                result.IsSuccess = false;
                result.Data = "";
                result.Message = "Not Deleted";
            }
            else
            {
                result.IsSuccess = true;
                result.Data = role;
                result.Message = "Found";
            }
            return Ok(result);
        }
    }
}
