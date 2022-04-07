using Core.Constants;
using Core.Contracts;
using Core.Models.Salaries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class SalaryController : BaseController
    {
        private readonly ISalaryService salaryService;

        public SalaryController (ISalaryService _salaryService)
        {
            salaryService = _salaryService;
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult All (int id = 1)
        {
            const int ItemsPerPage = 20;
            var viewModel = new SalaryListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                SalariesCount = salaryService.GetCountSalaries(),
                Salaries = salaryService.GetSalaries(id, ItemsPerPage),
            };
            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor}")]
        public IActionResult Add ()
        {
            var viewModel = new AddSalaryViewModel();
            viewModel.Employees = salaryService.GetAllEmployeesWithoutSalary();
            viewModel.FromDate = DateTime.Now;
            viewModel.ToDate = DateTime.Now;
            
            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor}")]
        [HttpPost]
        public async Task<IActionResult> Add (AddSalaryViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                model.Employees = salaryService.GetAllEmployeesWithoutSalary();
                return View(model);
            }

            bool created = false;

            try
            {
                created = await salaryService.AddSalaryAsync(model, created);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                await salaryService.AddSalaryAsync(model, created);
            }

            return Redirect("/Salary/Add");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult AllSalariesEmployee (string employeeId , int id = 1)
        {
            const int ItemsPerPage = 20;
            var viewModel = new SalaryEmployeeListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                SalariesCount = salaryService.GetCountSalaries(),
                Salaries = salaryService.GetSalariesEmployee(id, ItemsPerPage, employeeId),
                EmployeeName = salaryService.GetEmployeeName(employeeId)
            };
            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public IActionResult Edit (string salaryId)
        {
            var salary = salaryService.GetSalaryInfo(salaryId);

            return View(salary);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        [HttpPost]
        public async Task<IActionResult> Edit (EditSalaryViewModel model, string salaryId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await salaryService.EditSalaryAsync(model, salaryId);

            return Redirect("/Salary/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public async Task<IActionResult> Delete (string salaryId)
        {
            try
            {
                await salaryService.DeleteSalaryAsync(salaryId);

            }
            catch (Exception)
            {
                throw new Exception("Error for delete salary.");

            }

            return Redirect("/Salary/All");
        }
    }
}
