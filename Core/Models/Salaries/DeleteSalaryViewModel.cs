namespace Core.Models.Salaries
{
    public class DeleteSalaryViewModel
    {
        public string Id { get; set;}
        public int HoursWorked { get; set; }

        public decimal TotalAmount { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string EmployeeName { get; set; }
    }
}
