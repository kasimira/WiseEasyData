using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Salaries
{
    public class AddSalaryViewModel
    {
        [Required]
        public int HoursWorked { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Hourly salary must be a positive number.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        public string EmployeeName { get; set; }

        public IEnumerable<SelectListItem>? Employees { get; set; }

        public string? Note { get; set; }

        public decimal? InAdvance { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Hourly salary must be a positive number.")]
        public decimal Salary { get; set; }

    }
}
