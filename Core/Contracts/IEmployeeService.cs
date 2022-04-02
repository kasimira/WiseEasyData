using Core.Models.Employee;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IEmployeeService
    {
        int GetCount ();

        Task DeleteAsync (string employeeId);

        Task ChangeStatusAsync (string employeeId);

        public Employee GetEmployee (string employeeId);

        EmployeeProfileViewModel GetEmployeeProfil (string id);

        DeleteEmployeeViewModel GetEmployeeForDelete (string id);

        EditEmployeeViewModel GetEmployeeInfo<T> (string employeeId);

        IEnumerable<AllEmployeesViewModel> GetEmployees (int page, int itemsPerPage);

        Task EditEmployeeAsync (EditEmployeeViewModel model, string employeeId, string rootPath);       

        Task<bool> AddEmployeeAsync (AddEmployeeViewModel model, bool created, string rootPath, string userId, string userName);
    }
}
