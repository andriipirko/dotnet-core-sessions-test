using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebAPI.Services;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrintGreeting();

            if (!MySqlService.IsInstalled())
            {
                Console.WriteLine("MySQL server is not installed in your computer.");
                MySqlService.InstallMySql();
            }
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:5000")
                .Build();

        private static void PrintGreeting()
        {
            for (int i = 0; i < 80; i++)
                Console.Write("=");

            Console.WriteLine("\n\nThis web server is developped by Andrii Pirko, 2018.\n\n");

            for (int i = 0; i < 80; i++)
                Console.Write("=");

            Console.WriteLine("\n");
            Thread.Sleep(1000);
        }
    }
}
