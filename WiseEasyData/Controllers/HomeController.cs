using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WiseEasyData.Models;

namespace WiseEasyData.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController (ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index ()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/HomeApp/Index");
            }

            return View();
        }

        public IActionResult About ()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/HomeApp/Index");
            }

            return View();
        }

        public IActionResult Contact ()
        {
            return View();
        }

        public IActionResult Privacy ()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/HomeApp/Index");
            }

            return View();
        }

        public IActionResult StartMenu ()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/HomeApp/Index");
            }

            return View();
        }

        public IActionResult Profile ()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/HomeApp/Index");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
