using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class City
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public string CountryId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}

