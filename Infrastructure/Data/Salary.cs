using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Salary
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int HourlySalary { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

    }
}
