namespace Core.Models.Dashboard
{
    public class StatisticsAppViewModel
    {
        public IEnumerable<ExpenseViewModel> Expenses { get; set; }

        public decimal TotalExpenses { get; set; }

        public string Month { get; set; }
    }
}
