using Core.Models.Employee;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<AllEmployeesViewModel> GetEmployees (int page, int itemsPerPage);

        public Employee GetEmployee (string employeeId);

        Task EditEmployeeAsync (EditEmployeeViewModel model, string employeeId, string rootPath);

        Task DeleteAsync (string employeeId);

        Task<bool> AddEmployeeAsync (AddEmployeeViewModel model, bool created, string rootPath, string userId, string userName);

        int GetCount ();

        EditEmployeeViewModel GetEmployeeInfo<T> (string employeeId);

        EmployeeProfileViewModel GetEmployeeProfil (string id);

        Task ChangeStatusAsync (string employeeId);
    }
}
