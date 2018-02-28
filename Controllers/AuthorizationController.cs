using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Authorization")]
    public class AuthorizationController : Controller
    {
        [HttpPost]
        public void Post(string name = null, string password = null)
        {
            if (name == "Bogdan" && password == "KpacaBa")
            {
                HttpContext.Session.SetInt32("authorizated", 1);
            }
            else
            {
                HttpContext.Session.SetInt32("authorizated", 0);
            }
        }

        [HttpGet]
        public string Get()
        {
            if (HttpContext.Session.GetInt32("authorizated") == 1)
            {
                return "Бодя Яворський чоткий пацан.";
            }
            else
            {
                return "Спробуй ще разок :)";
            }
        }
    }
}