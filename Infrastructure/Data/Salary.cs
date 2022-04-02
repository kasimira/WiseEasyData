using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Salary
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Range(1, 100000)]
        public decimal HourlySalary { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal TotalAmount { get; set; }

        [Required]
        [Range(1, 100000)]
        public decimal SalaryAmount { get; set; }

        [Range(0, 100000)]
        public decimal? InAdvance { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public string? Note { get; set; }
    }
}
