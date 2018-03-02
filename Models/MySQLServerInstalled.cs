using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace WebAPI.Models
{
    public class MySqlServerInstalled
    {
        public bool IsInstalled()
        {
            var result = ServiceController.GetServices();
            var result_2 = result.Select(m => m.ServiceName).Where(m => m.ToLower().Contains("mysql"));
            return result_2.ToList().Count != 0;
        }
    }
}
