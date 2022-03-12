using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(10)]
        public string Nationality { get; set; }

        [StringLength(10)]
        public string? EGN { get; set; }

        public int? NumberOfPersonalId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }  

        [Required]
        [StringLength(45)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Position { get; set; }
        [Required]
        [StringLength(20)]
        public string Department{ get; set; }

        [Required]
        public DateTime HireDate { get; set; }
        
        public DateTime? ReleaseDate { get; set; }

        [StringLength(255)]
        public string? PhotoPath { get; set; }

        public Guid SalaryId = Guid.NewGuid();

        public Salary Salary { get; set; }


        [StringLength(20)]
        public string Grade { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]      
        public Guid CityId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]      
        public Guid CountryId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }   
        

        public bool IsDeleted { get; set; } = false;


    }
}
