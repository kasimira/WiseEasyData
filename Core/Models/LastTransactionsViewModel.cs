using Infrastructure.Data.Enums;

namespace Core.Models
{
    public class LastTransactionsViewModel
    {
        public string Date { get; set; }

        public string Name { get; set; }
        public TransactionType Type { get; set; }
    }
}

