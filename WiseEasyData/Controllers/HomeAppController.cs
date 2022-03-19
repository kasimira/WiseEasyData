using Core.Contracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class HomeAppController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIndexAppService indexAppService;
        public HomeAppController(ILogger<HomeController> logger, IIndexAppService _indexAppService)
        {
            _logger = logger;
            indexAppService = _indexAppService;
        }

        public IActionResult Index()
        {
            var infoDashboard = indexAppService.GetInfo();

            return View(infoDashboard);
        }
    }
}
