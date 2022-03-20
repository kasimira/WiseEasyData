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
            _logger = logger;
            employeeService = _employeeService;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All(int id = 1)
        {
            const int ItemsPerPage = 2;
            var viewModel = new EmployeesListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EmployeeCount = employeeService.GetCount(),
                Employees = employeeService.GetEmployees(id, ItemsPerPage),
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
            var rootPath = webHostEnvironment.WebRootPath;

            try
            {
                created = await employeeService.AddEmployeeAsync(model, created, rootPath);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                await employeeService.AddEmployeeAsync(model, created, rootPath);
            }

            if (!created)
            {
                return Redirect("/Employee/Error");
            }

            return Redirect("/Employee/All");
        }

       public IActionResult Edit(string employeeId)
       {
           var employee = employeeService.GetEmployeeInfo<EditEmployeeViewModel>(employeeId);
      
           return View(employee);
       }

       [HttpPost]
       public async Task<IActionResult> Edit(EditEmployeeViewModel model, string employeeId)
       {
            

            if (!ModelState.IsValid)
           {
       
               //return Redirect($"/Employee/Edit/Id={employeeId}");
               return this.View(model);
           }

            var rootPath = webHostEnvironment.WebRootPath;

            await employeeService.EditEmployeeAsync(model, employeeId, rootPath);
       
           return Redirect("/Employee/All");
       }

        public async Task<IActionResult> Delete(string employeeId)
        {
            await employeeService.DeleteAsync(employeeId);

            return Redirect("/Employee/All");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
