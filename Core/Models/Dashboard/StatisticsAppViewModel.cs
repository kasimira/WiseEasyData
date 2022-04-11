namespace Core.Models.Dashboard
{
    public class StatisticsAppViewModel
    {
        public IEnumerable<ExpenseViewModel>? Expenses { get; set; }

        public IEnumerable<IncomesViewModel>? Incomes { get; set; }

        public SalariesViewModel? Salaries { get; set; }

        public decimal TotalAmounth { get; set; }

        public string Month { get; set; }
    }
}
