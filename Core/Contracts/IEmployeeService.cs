using Core.Models;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IEmployeeService
    {
        //Task (bool created, string? error) AddEmployee(AddEmployeeViewModel model);

        IEnumerable<AllEmployeesViewModel> GetEmployees(int page, int itemsPerPage);

        //(bool edited, string error) EditEmployee(EditEmployeeViewModel model, Guid employeeId);
        EditEmployeeViewModel GetEmployeeForChange(string employeeId);

        public Employee GetEmployee(string employeeId);
        Task EditEmployeeAsync(AddEmployeeViewModel model, string employeeId);
        Task DeleteAsync(string employeeId);
        Task<bool> AddEmployeeAsync(AddEmployeeViewModel model, bool created);

        int GetCount();
    }
}
