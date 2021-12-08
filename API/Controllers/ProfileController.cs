using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Customer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Customer;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProfileController : ControllerBase
    {
        IAppUserRepository CustomerRepository;
        Result Result;
        public ProfileController(IAppUserRepository customerRepository)
        {
            CustomerRepository = customerRepository;
            Result = new Result();
        }
        [HttpGet("MyProfile")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                string id = User.Claims.FirstOrDefault(i => i.Type == "UserID").Value;
                var user = await CustomerRepository.GetCustomerData(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Result.IsSuccess = false;
                Result.Data = ex.Data;
                Result.Message = ex.Message;
                return Ok(Result);
            }
        }
        [HttpPut("Edit")]
        public async Task<IActionResult> EditUserProfile([FromBody]EditCusomerViewModel editCusomerViewModel)
        {
            try
            {
                editCusomerViewModel.Id = User.Claims.FirstOrDefault(i => i.Type == "UserID").Value;
                var user = await CustomerRepository.EditProfile(editCusomerViewModel);
                if (user == null)
                    return Ok("Empty");
                return Ok(user);
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
