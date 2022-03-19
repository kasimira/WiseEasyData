using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class CostController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public CostController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult AllCostsByCategory()
        {
            return View();
        }
    }
}
