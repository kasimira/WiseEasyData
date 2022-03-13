using Core.Contracts;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WiseEasyData.Models;

namespace WiseEasyData.Controllers
{
    public class EmployeeController : BaseController
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService employeeService;



        public EmployeeController(ILogger<HomeController> logger, IEmployeeService _employeeService)
        {
            _logger = logger;
            employeeService = _employeeService;
           
        }

        public IActionResult All(AllEmployeesViewModel model)
        {
            var employees = employeeService.GetEmployees(); 
            return View(employees);
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

            var (created, error) = employeeService.AddEmployee(model);

            if (!created)
            {
                
                return View("EmployeeController/Error");
            }

            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
