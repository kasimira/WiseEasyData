using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class ChatController : BaseController
    {
        public ChatController ()
        {
           
        }

        public IActionResult Index ()
        {
        
            return View();
        }
    }
}
