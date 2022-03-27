using Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WiseEasyData.Controllers
{
    public class HomeAppController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIndexAppService indexAppService;
        public HomeAppController (ILogger<HomeController> logger, IIndexAppService _indexAppService)
        {
            _logger = logger;
            indexAppService = _indexAppService;
        }

        public IActionResult Index ()
        {
            var infoDashboard = indexAppService.GetInfo();
            var dataPoints = indexAppService.GetDataPoint();

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(infoDashboard);
        }
    }
}
