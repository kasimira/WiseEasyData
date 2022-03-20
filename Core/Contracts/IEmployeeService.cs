using Core.Models;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IEmployeeService
    {
        //Task (bool created, string? error) AddEmployee(AddEmployeeViewModel model);

        IEnumerable<AllEmployeesViewModel> GetEmployees(int page, int itemsPerPage);

        //(bool edited, string error) EditEmployee(EditEmployeeViewModel model, Guid employeeId);       

        public Employee GetEmployee(string employeeId);
        Task EditEmployeeAsync(EditEmployeeViewModel model, string employeeId, string rootPath);
        Task DeleteAsync(string employeeId);
        Task<bool> AddEmployeeAsync(AddEmployeeViewModel model, bool created, string rootPath);

        int GetCount();
        //EditEmployeeViewModel GetEmployeeInfo(EditEmployeeViewModel model, string employeeId);
        EditEmployeeViewModel GetEmployeeInfo<T>(string employeeId);
    }
}
