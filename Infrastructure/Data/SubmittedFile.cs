using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class SubmittedFile
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string OwnerId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Extension { get; set; }

        public DateTime DateToAdd { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public bool IsImage { get; set; } = true;

        public string? TransactionName { get; set; }
            
        ////The contents of the image is in the file system
    }
}
