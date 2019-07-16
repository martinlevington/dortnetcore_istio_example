using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using docker_helloWorld.Models;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace docker_helloWorld.Controllers
{

    
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var msg = new HelloViewModel
            { Msg = "Hello World" };

            return View(msg);
        }

        [Route("ping")]
        public async Task<IActionResult> Ping()
        {
            var serviceUrl = _configuration["PingService:Url"];
            _logger.LogInformation("serviceUrl: " + serviceUrl);

            var msg = new HelloViewModel
            { Msg = "Ping " };

            var client = new HttpClient();
           
            var stringTask = client.GetStringAsync(serviceUrl+"ping");

            msg.Msg = await stringTask;


            _logger.LogInformation("End of ping");

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
