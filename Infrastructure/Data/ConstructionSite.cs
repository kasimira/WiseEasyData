using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class ConstructionSite
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(250)]
        public string? Name { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(1, 200)]
        public decimal HourlyRate { get; set; }

        [Required]
        [Range(1, 20)]
        public double WorkingHoursOnWeekdays { get; set; }

        [Required]
        [Range(1, 20)]
        public double WorkingHoursOnWeekends { get; set; }

        [Required]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        public string CityId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }

        [Required]
        [StringLength(20)]
        public string? PostalCode { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
