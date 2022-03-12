using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class City
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
         
        [Required]
        public Guid CountryId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public ICollection<Employee> Employees { get; set;} = new List<Employee>(); 
       
    }
}
