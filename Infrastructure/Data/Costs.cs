using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Costs
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Category { get; set; }

        [StringLength(700)]
        public string? Description { get; set; }

        [Required]
        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
