using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Cart;
using Models.Customer;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModels.Cart;
using ViewModels.Customer;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAppUserRepository appUserRepository;
        IMainRepository<CartEntity> CartRepository;
        Result Result;
        public AccountController(IAppUserRepository _appUserRepository,
            IMainRepository<CartEntity> cartRepo)
        {
            appUserRepository = _appUserRepository;
            CartRepository = cartRepo;
            Result = new Result();
        }

        #region Normal REST Customer
        /*
        [HttpGet()]
        public async Task<string> GetAsync()
        {
            Result.Data = await CustomerRepository.Get();
            string jsonObject = JsonConvert.SerializeObject(Result, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonObject;
        }
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            Result.Data = await CustomerRepository.Get(id);
            string jsonObject = JsonConvert.SerializeObject(Result, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonObject;
        }
        [HttpPost()]
        public string Post(CustomerVM addCustomer)
        {
            // create object of model and convert it to view model
            // to custmoer model in db
            CustomerEntity customerEntity = addCustomer.ToCusomerModel();
            customerEntity =  CustomerRepository.Add(customerEntity).Result;
            // create an object from cart view model
            AddCart addCart = new AddCart()
            {
                CustomerID = customerEntity.ID,
                Status = CartStatus.Cleared,
                TotalPrice = 0.0f,
                CustomerEntity = customerEntity,
            };
            // conver view cart view model to cart model in db
            CartEntity cartEntity = addCart.ToCartEntity();
            // add cart model into db
            CartRepository.Add(cartEntity);
            // create object of returned result
            Result.Data = new { 
                Customer = customerEntity,
                Cart = cartEntity
            };
            string jsonObject = JsonConvert.SerializeObject(Result,Formatting.None,new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonObject ;
        }
        [HttpPut()]
        public async Task<Result> Put(CustomerVM addCustomer)
        {
            Result.Data = await CustomerRepository.Update(addCustomer.ToCusomerModel());
            return Result;
        }
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            CustomerEntity customerEntity = CustomerRepository.Delete(id).Result;
            Result.Data = new
            {
                Customer = customerEntity,
            };
            string jsonObject = JsonConvert.SerializeObject(Result, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return jsonObject;
        }
        [HttpGet("Login")]
        public string Login(string username,string password)
        {
            IEnumerable<CustomerEntity> customers = CustomerRepository.Get().Result;
            CustomerEntity selectedCustomer = customers.FirstOrDefault(i => i.Username == username && i.Password == password);
            if(selectedCustomer == null)
            {
                return "Invalid Username Or Password";
            }
            else
            {
                return "Welcome";
            }
        }*/
        #endregion

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUP(SignUpViewModel signUpViewModel)
        {
            try
            {
                Result = await appUserRepository.Signup(signUpViewModel);
                if (Result.IsSuccess == false)
                    return Unauthorized(Result);
                if (signUpViewModel.UserRole == "Customer")
                {
                    // create a cart for each customer
                    CartEntity cartEntity = new CartEntity()
                    {
                        CustomerID = (Result.Data as ApplicationUserEntity).Id,
                        Status = CartStatus.Cleared,
                        CustomerEntity = (Result.Data as ApplicationUserEntity),
                        TotalPrice = 0.0f
                    };
                    // add cart in database
                    cartEntity = await CartRepository.Add(cartEntity);
                    object obj = new
                    {
                        Customer = Result.Data,
                        Cart = cartEntity.ToViewModel()
                    };
                    // return new user and its car
                    string jsonObject = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    Result.Data = jsonObject;
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
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery]LoginViewModel loginViewModel)
        {
            try
            {
                Result = await appUserRepository.Login(loginViewModel);

                if (Result.IsSuccess == false)
                    return Unauthorized(Result);
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
        [HttpPost("ForegetPassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery]string Email)
        {

            try
            {
                Result = await appUserRepository.ForegetPassword(Email);
                if (Result.IsSuccess == false)
                    return Unauthorized(Result);
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
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery]ResetPasswordViewModel resetPasswordViewModel)
        {
            try
            {
                Result = await appUserRepository.RestPassword(resetPasswordViewModel);
                if (Result.IsSuccess == false)
                    return Unauthorized(Result);
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
