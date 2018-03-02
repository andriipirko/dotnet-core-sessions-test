using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Mysql")]
    public class MysqlController : Controller
    {
        [HttpGet]
        public string Index()
        {
            var mysqlController = new MySqlServerInstalled();
            return mysqlController.IsInstalled() ? "installed" : "not installed";
        }
    }
}