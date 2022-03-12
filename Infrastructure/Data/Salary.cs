using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Salary
    {
        [Key]
        public Guid SalaryId { get; set; } = Guid.NewGuid();

        [Required]
        public int HourlySalary { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }    

    }
}
