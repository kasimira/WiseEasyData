using Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Employee
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string? Nationality { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(45)]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(20)]
        public string? Position { get; set; }

        [Required]
        public int HourlySalary { get; set; }

        [Required]
        public string DepartmentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime DateToAdd { get; set; } = DateTime.UtcNow;

        public string? ImageId { get; set; } = Guid.NewGuid().ToString();

        [StringLength(20)]
        public Grade Grade { get; set; }

        [StringLength(10)]
        public Status Status { get; set; }

        [Required]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        public string CityId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }

        [Required]
        [StringLength(20)]
        public string? PostalCode { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Salary> Salaries { get; set; } = new List<Salary>();
    }
}
