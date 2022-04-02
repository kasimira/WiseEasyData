namespace Core.Models.Salaries
{
    public class AllSalariesEmployeeViewModel
    {
        public string Id { get; set; }

        public decimal HourlySalary { get; set; }

        public int HoursWorked { get; set; }

        public decimal TotalAmount { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string EmployeeId { get; set; }

        public decimal? InAdvance { get; set; }

        public decimal TransferredPaymen { get; set; }
    }
}
