using Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Client
{
    public class AddClientViewModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Vat Number")]
        [StringLength(20, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string? VatNumber { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Ceo Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string? CeoName { get; set; }

        [StringLength(45)]
        public string? PhoneNumber { get; set; }

        [StringLength(45)]
        public string? MobilePhone { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email must be valid email.")]
        public string? Email { get; set; }

        [EmailAddress]
        public string? Email2 { get; set; }

        public string? Website { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(255)]
        public string? LogoPath { get; set; }

        [Required]
        [EnumDataType(typeof(Status))]
        public string? Status { get; set; }

        [Required]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string? City { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Postal code")]
        public string? PostalCode { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string? Country { get; set; }
    }
}
