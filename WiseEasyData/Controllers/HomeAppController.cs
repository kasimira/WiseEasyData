using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class HomeAppController : BaseController
    {

        private readonly ILogger<HomeController> _logger;

        public HomeAppController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
          
            return View();
        }

        
        public IActionResult Profile()
        {

            return View();
        }

    }
}
