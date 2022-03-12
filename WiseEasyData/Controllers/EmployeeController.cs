using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class EmployeeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        

        public EmployeeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           
        }

        public IActionResult All()
        {

            return View();
        }


        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddEmployeeViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return this.View();
            }

            return Redirect("/");
        }

    }
}
