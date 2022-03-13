using Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class AddEmployeeViewModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Nationality { get; set; }

        [StringLength(10)]
        public string? EGN { get; set; }

        public int? NumberOfPersonalId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Middle Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [RegularExpression("[A-Z][^_]+", ErrorMessage = "Name should start with upper letter. ")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public string Gender { get; set; }

        [Required]
        [StringLength(45)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email must be valid email.")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Position { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Department { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }
       
        public IFormFile? Image { get; set; }

        [Required]
        [Range(1,1000, ErrorMessage = "Hourly salary must be a positive number between 1 and 1000.")]
        public int Salary { get; set; }

        [Required]
        [EnumDataType(typeof(Grade))]
        public string Grade { get; set; }

        [Required]
        [EnumDataType(typeof(Status))]
        public string Status { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }
   
        public string City { get; set; }


        [Required]
        [StringLength(20)]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }
   
        public string Country { get; set; }

    }
}
