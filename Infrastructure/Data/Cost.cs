using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Cost
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string? CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public CategoryCost? CategoryCost { get; set; }

        [StringLength(700)]
        public string? Description { get; set; }

        [Required]
        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

    }
}
