using Core.Constants;
using Core.Contracts;
using Core.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WiseEasyData.Models;

namespace WiseEasyData.Controllers
{ 
    public class EmployeeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService employeeService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeController (ILogger<HomeController> logger, IEmployeeService _employeeService, IWebHostEnvironment _webHostEnvironment)
        {
            _logger = logger;
            employeeService = _employeeService;
            webHostEnvironment = _webHostEnvironment;
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult All (int id = 1)
        {
            const int ItemsPerPage = 6;

            var viewModel = new EmployeesListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EmployeeCount = employeeService.GetCount(),
                Employees = employeeService.GetEmployees(id, ItemsPerPage),
            };

            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult Profile (string employeeId)
        {
            var viewModel = employeeService.GetEmployeeProfil(employeeId);

            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public async Task<IActionResult> ChangeStatus (string employeeId)
        {
            await employeeService.ChangeStatusAsync(employeeId);

            return Redirect("/Employee/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor}")]
        public IActionResult Add ()
        {
            return View();
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        [HttpPost]
        public async Task<IActionResult> Add (AddEmployeeViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userName = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool created = false;
            var rootPath = webHostEnvironment.WebRootPath;

            try
            {
                created = await employeeService.AddEmployeeAsync(model, created, rootPath, userId, userName);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                await employeeService.AddEmployeeAsync(model, created, rootPath, userId, userName);
            }

            if (!created)
            {
                return Redirect("/Employee/Error");
            }

            return Redirect("/Employee/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public IActionResult Edit (string employeeId)
        {
            var employee = employeeService.GetEmployeeInfo<EditEmployeeViewModel>(employeeId);

            return View(employee);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        [HttpPost]
        public async Task<IActionResult> Edit (EditEmployeeViewModel model, string employeeId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var rootPath = webHostEnvironment.WebRootPath;

            await employeeService.EditEmployeeAsync(model, employeeId, rootPath);

            return Redirect("/Employee/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public async Task<IActionResult> Delete (string employeeId)
        {
            await employeeService.DeleteAsync(employeeId);

            return Redirect("/Employee/All");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
