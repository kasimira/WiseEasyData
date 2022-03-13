using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IEmployeeService
    {
        (bool created, string error) AddEmployee(AddEmployeeViewModel model);

        IEnumerable<AllEmployeesViewModel> GetEmployees();
    }
}
