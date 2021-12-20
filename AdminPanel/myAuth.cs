using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel
{
    public class myAuth :  Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var roles = context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserID").Value;
            string token = context.HttpContext.Session.GetString("Token");

            bool test = context.HttpContext.User.IsInRole("Admin");
        }
    }
}
