using Core.Models.Salaries;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Contracts
{
    public interface ISalaryService
    {
        public int GetCountSalaries ();

        public IEnumerable<AllSalariesViewModel> GetSalaries (int id, int itemsPerPage);

        IEnumerable<SelectListItem>? GetAllEmployees ();

        Task<bool> AddSalaryAsync (AddSalaryViewModel model, bool created);

        IEnumerable<SelectListItem>? GetAllEmployeesWithoutSalary ();

        IEnumerable<AllSalariesEmployeeViewModel> GetSalariesEmployee (int id, int itemsPerPage, string employeeId);

        string GetEmployeeName (string employeeId);

        EditSalaryViewModel GetSalaryInfo (string salaryId);

        Task EditSalaryAsync (EditSalaryViewModel model, string salaryId);
        Task DeleteSalaryAsync (string salaryId);
    }
}
