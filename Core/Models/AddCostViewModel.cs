using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class AddCostViewModel
    {

        [Required]
        public string? Category { get; set; }

        [StringLength(700)]
        public string? Description { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

    }
}

