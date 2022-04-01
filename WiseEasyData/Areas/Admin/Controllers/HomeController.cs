using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index ()
        {
            return View();
        }
    }
}
