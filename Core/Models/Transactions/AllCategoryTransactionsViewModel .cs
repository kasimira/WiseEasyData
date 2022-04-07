using Infrastructure.Data.Enums;

namespace Core.Models.Transactions
{
    public class AllCategoryTransactionsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FileId { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Date { get; set; }

        public string Category { get; set; }

        public string CreatorName { get; set; }
    }
}
