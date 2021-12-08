using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Customer;
using Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Result Result;
        private readonly UserManager<ApplicationUserEntity> userManager;

        public UserRoleController(RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUserEntity> userManager) {
            this.roleManager = roleManager;
            this.Result = new Result();
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var roles = roleManager.Roles;

                if (roles == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Not Found";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = roles.ToList();
                    Result.Message = "Found";
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
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(name);
                if (role == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Not Found";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = role;
                    Result.Message = "Found";
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
        [HttpPost("{name}")]
        public async Task<IActionResult> Add(string name)
        {
            try
            {
                var role = new IdentityRole()
                {
                    Name = name
                };
                var res = await roleManager.CreateAsync(role);
                if (res == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Not Added";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = await roleManager.FindByNameAsync(name);
                    Result.Message = "Found";
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
        [HttpDelete("{name}")]
        public async Task<IActionResult> delete(string name)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(name);
                var res = await roleManager.DeleteAsync(role);
                if (res == null)
                {
                    Result.IsSuccess = false;
                    Result.Data = "";
                    Result.Message = "Not Deleted";
                }
                else
                {
                    Result.IsSuccess = true;
                    Result.Data = role;
                    Result.Message = "Found";
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
