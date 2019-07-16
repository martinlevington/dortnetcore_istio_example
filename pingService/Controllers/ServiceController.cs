using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pingService.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pingService.Controllers
{



    public class ServiceController : Controller
    {
        private readonly ILogger _logger;

        public ServiceController(ILogger<ServiceController> logger)
        {
            _logger = logger;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Route("ping")]
        [HttpGet]
        public PongMessage Ping(string message)
        {
            _logger.LogInformation("Got ping request. Message: {}", message);
            return new PongMessage(message);
        }
    }
}
