using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class File
    {
        [Key]
        public Guid FileId { get; set; } = Guid.NewGuid();

        [Required]
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
