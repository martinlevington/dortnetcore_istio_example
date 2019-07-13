using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using docker_helloWorld.Models;

namespace docker_helloWorld.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var msg = new HelloViewModel
            { Msg = "Hello World" };

            return View(msg);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
