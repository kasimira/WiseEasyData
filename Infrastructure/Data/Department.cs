using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Department
    {
        public Department ()
        {
            Employees= new List<Employee> ();   
        }
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
