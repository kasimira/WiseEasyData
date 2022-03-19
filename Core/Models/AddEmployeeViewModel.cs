using Core.CustomAttributes;
using Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class AddEmployeeViewModel
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string? Nationality { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public string? Gender { get; set; }

        [Required]
        [StringLength(45)]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email must be valid email.")]
        public string? Email { get; set; }

        [Required]
        [StringLength(20)]
        public string? Position { get; set; }

        [Required]
        public string? Department { get; set; }

        [Required]
        [DataType(DataType.Date)]      
        public DateTime HireDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [IsImage(ErrorMessage = "Please select only Supported Files .png | .jpg. Flite must by maximum 10mb.")] 
        public IFormFile? Image { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Hourly salary must be a positive number between 1 and 1000.")]
        public int HourlySalary { get; set; }

        [Required]
        [EnumDataType(typeof(Grade))]
        public string? Grade { get; set; }

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

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string? Country { get; set; }

    }
}
