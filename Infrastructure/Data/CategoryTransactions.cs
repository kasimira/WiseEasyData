using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data
{
    public class CategoryTransactions
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(250)]
        public string? Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
