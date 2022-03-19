using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Department
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
