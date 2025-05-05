using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //       [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }
        //  [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

    }
}

