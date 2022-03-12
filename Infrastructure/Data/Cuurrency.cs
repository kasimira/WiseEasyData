using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class Cuurrency
    {
        [Key]
        public Guid CuurrencyId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(6)]
        public string CurrencyCode { get; set; }

        [DataType(DataType.Currency)]
        public float? Cost { get; set; }
        
    }
}
