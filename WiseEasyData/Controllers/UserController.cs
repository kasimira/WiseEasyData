using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        public UserController (ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult All ()
        {
            return View();
        }
    }
}
