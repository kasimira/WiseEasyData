using Core.Contracts;
using Core.Models.Salaries;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Core.Services
{
    public class SalaryService : ISalaryService
    {
        private readonly IApplicatioDbRepository repo;

        public SalaryService (IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> AddSalaryAsync (AddSalaryViewModel model, bool created)
        {
            var employeeId = model.EmployeeName;
            var employee = repo.All<Employee>().Where(e => e.Id == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception("Employee is null.");
            }

            var salary = new Salary()
            {
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                SalaryAmount = model.Salary,
                TotalAmount = model.TotalAmount,
                InAdvance = model.InAdvance,
                HourlySalary = employee.HourlySalary,
                HoursWorked = model.HoursWorked,
                EmployeeId = employeeId,
                Employee = employee
            };

            if (salary == null)
            {
                throw new Exception("Salary is null.");
            }

            employee.Salaries.Add(salary);

            try
            {
                await repo.AddAsync(salary);
                await repo.SaveChangesAsync();
                created = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (created);
        }

        public IEnumerable<SelectListItem>? GetAllEmployees ()
        {
            var query = repo.AllReadonly<Employee>()
               .Where(x => x.IsDeleted == false)
               .OrderBy(x => x.FirstName)
               .Select(c => new SelectListItem
               {
                   Value = c.Id,
                   Text = c.FirstName + " " + c.LastName,
               }).ToList();

            return query;
        }

        public IEnumerable<SelectListItem>? GetAllEmployeesWithoutSalary ()
        {
            var query = repo.AllReadonly<Employee>()
               .Where(x => x.IsDeleted == false)
               .Where(x => x.Salaries.All(s => s.FromDate.Month != DateTime.Now.Month - 1))
               .OrderBy(x => x.FirstName)
               .Select(c => new SelectListItem
               {
                   Value = c.Id,
                   Text = c.FirstName + " " + c.LastName,
               }).ToList();

            return query;
        }

        public int GetCountSalaries ()
        {
            return repo.AllReadonly<Salary>().Count();
        }

        public IEnumerable<AllSalariesViewModel> GetSalaries (int page, int itemsPerPage)
        {
            var countSalaries = GetCountSalaries();

            if (itemsPerPage > countSalaries)
            {
                itemsPerPage = countSalaries;
            }

            return repo.AllReadonly<Salary>()
                .OrderByDescending(t => t.TotalAmount)
                .Where(s => s.FromDate.Month == DateTime.Now.Month -1)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(s => new AllSalariesViewModel()
                {
                    Id = s.Id,
                    TotalAmount = s.TotalAmount,
                    HourlySalary = s.HourlySalary,
                    FromDate = s.FromDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    ToDate = s.ToDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    HoursWorked = s.HoursWorked,
                    EmployeeId = s.EmployeeId,
                    EmployeeName = s.Employee!.FirstName + " " + s.Employee.LastName,
                    InAdvance = s.InAdvance,
                    TransferredPaymen = s.SalaryAmount,
                })
                .ToList();
        }

        public IEnumerable<AllSalariesEmployeeViewModel> GetSalariesEmployee (int page, int itemsPerPage, string employeeId)
        {
            var countSalaries = GetCountSalaries();

            if (itemsPerPage > countSalaries)
            {
                itemsPerPage = countSalaries;
            }

            return repo.AllReadonly<Salary>()
                .Where(s => s.EmployeeId == employeeId)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(s => new AllSalariesEmployeeViewModel()
                {
                    Id = s.Id,
                    TotalAmount = s.TotalAmount,
                    HourlySalary = s.HourlySalary,
                    FromDate = s.FromDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    ToDate = s.ToDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    HoursWorked = s.HoursWorked,
                    EmployeeId = s.EmployeeId,
                    InAdvance = s.InAdvance,
                    TransferredPaymen = s.SalaryAmount,
                })
                .ToList();
        }

        public string GetEmployeeName (string employeeId)
        {
            var employeeName = repo.AllReadonly<Employee>()
                .Where(e => e.Id == employeeId)
                .Select(e => e.FirstName + " " + e.LastName).FirstOrDefault();

            return employeeName;
        }

        public EditSalaryViewModel GetSalaryInfo (string salaryId)
        {
           var salary = repo.AllReadonly<Salary>()
                .Where(s => s.Id == salaryId)
                .Select(s => new EditSalaryViewModel()
                {
                    InAdvance = s.InAdvance,
                    TotalAmount = s.TotalAmount,
                    EmployeeName = s.Employee!.FirstName + " " + s.Employee.LastName,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate,
                    Salary = s.SalaryAmount,
                    HoursWorked = (int)s.HoursWorked,
                    Note = s.Note

                }).FirstOrDefault();

            return salary;
        }

        public async Task EditSalaryAsync (EditSalaryViewModel model, string salaryId)
        {
            var salary = repo.All<Salary>()
                .Where(s => s.Id == salaryId)
                .FirstOrDefault();

            salary!.InAdvance = model.InAdvance;
            salary.Note = model.Note;
            salary.SalaryAmount = model.Salary;
            salary.FromDate = model.FromDate;
            salary.ToDate = model.ToDate;
            salary.HoursWorked = model.HoursWorked;
            salary.TotalAmount = model.TotalAmount;
            salary.InAdvance = model.InAdvance;

            await repo.SaveChangesAsync();
        }
    }
}
