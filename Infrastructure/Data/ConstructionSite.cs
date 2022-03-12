using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ConstructionSite
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(1, 20)]
        public decimal HourlyRate { get; set; }

        [Required]
        [Range(1, 20)]
        public double WorkingHoursOnWeekdays { get; set; }

        [Required]
        [Range(1,20)]
        public double WorkingHoursOnWeekends { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public Guid CityId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        public Guid CountryId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
