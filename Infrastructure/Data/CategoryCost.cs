using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class CategoryCost
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(250)]
        public string? Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Cost> Costs { get; set; } = new List<Cost>();
    }
}
