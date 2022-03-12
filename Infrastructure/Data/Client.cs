using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(20)]
        public string VatNumber  { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string CeoName { get; set; }
        
        [StringLength(45)]
        public string PhoneNumber { get; set; }
        
        [StringLength(45)]
        public string MobilePhone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [EmailAddress]
        public string? Email2 { get; set; }
       
        public string? Website { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [StringLength(255)]
        public string? LogoPath { get; set; }     

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

        public ICollection<ConstructionSite> constructionsSites { get; set; } = new List<ConstructionSite>();
    }
}
