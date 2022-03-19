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
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeController(ILogger<HomeController> logger, IEmployeeService _employeeService, IWebHostEnvironment _webHostEnvironment)
        {
            this._logger = logger;
            this.employeeService = _employeeService;
            this.webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 2;
            var viewModel = new EmployeesListViewModel
            {
                ItemsPerPage = ItemsPerPage,    
                PageNumber = id,
                EmployeeCount = this.employeeService.GetCount(),
                Employees = this.employeeService.GetEmployees(id , ItemsPerPage),
        };
            return View(viewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool created = false;

            using (FileStream fs = new FileStream(this.webHostEnvironment.WebRootPath + "/employee.png", FileMode.Create))
            {
                if(model.Image != null)
                {
                    await model.Image.CopyToAsync(fs);
                }

            }

            created = await employeeService.AddEmployeeAsync(model, created);

            if (!created)
            {
                return Redirect("/Employee/Error");
            }

            return Redirect("/Employee/All");
        }

        public IActionResult Edit(string employeeId)
        {
            var employee = employeeService.GetEmployeeForChange(employeeId);

            if (employee == null)
            {
                // ModelState.AddModelError(ErrorMessage = "Error", employee);
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(AddEmployeeViewModel model, string employeeId)
        {
            if (!ModelState.IsValid)
            {
                return this.View();
            }
        
            await this.employeeService.EditEmployeeAsync(model,employeeId);
                   
            return Redirect("/Employee/All");
        }

        public async Task<IActionResult> Delete(string employeeId)
        {
            await this.employeeService.DeleteAsync(employeeId);

            return Redirect("/Employee/All");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
