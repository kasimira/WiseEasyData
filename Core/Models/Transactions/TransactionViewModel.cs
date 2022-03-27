using Infrastructure.Data.Enums;

namespace Core.Models.Transactions
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string? File { get; set; }

        public string FileId { get; set; }

        public string ImageFile { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime Date { get; set; }

        public DateTime DataToAdd { get; set; } = DateTime.UtcNow;

        public string CreatorName { get; set; }
    }
}
