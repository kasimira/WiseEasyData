using Infrastructure.Data.Enums;

namespace Core.Models.Dashboard
{
    public class LastTransactionsViewModel
    {
        public string Date { get; set; }

        public string Name { get; set; }
        public TransactionType Type { get; set; }
    }
}
