using System.ComponentModel.DataAnnotations;

namespace Core.Models.Transactions
{
    public class AddCategoryTransactionViewModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
