using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class Cuurrency
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(30)]
        public string? Name { get; set; }

        [Required]
        [StringLength(6)]
        public string? CurrencyCode { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float? Cost { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
