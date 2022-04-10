using Core.Models.Employee;

namespace Core.Models.Dashboard
{
    public class IndexAppViewModel
    {
        public int EmployeeCount { get; set; }

        public int UsersCount { get; set; }

        public decimal TotalExpenses { get; set; }

        public decimal TotalIncomes { get; set; }

        public decimal TotalSalary { get; set; }

        public int ClientCount { get; set; }

        public decimal TotalExpensesMonth { get; set; }

        public decimal TotalIncomesMonth { get; set; }

        public IEnumerable<ExpenseViewModel> Expenses { get; set; }

        public IEnumerable<LastTransactionsViewModel> LastTransactions { get; set; }

        public IEnumerable<LastAddedEmployeesViewModel> LastEmployees { get; set; }
    }
}
