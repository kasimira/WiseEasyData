using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Country
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
