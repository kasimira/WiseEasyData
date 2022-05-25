using Infrastructure.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Data
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string? FileId { get; set; }

        [Required]
        public string CategoryId { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(CategoryId))]
        public CategoryTransactions Category { get; set; }

        [StringLength(700)]
        public string? Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public string CreatorId { get; set; } = Guid.NewGuid().ToString();

        public string CreatorName { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ClientId { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

    }
}

