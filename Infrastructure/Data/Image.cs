using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Image
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Extension { get; set; }

        public DateTime DateToAdd { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        ////The contents of the image is in the file system
    }
}
