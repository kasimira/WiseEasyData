using Core.Models.Transactions;

namespace Core.Contracts
{
    public interface ITransactionService
    {
        IEnumerable<AllTransactionsViewModel> GetTransactions (int page, int itemsPerPage);

        int GetCountTransactions ();

        Task<bool> AddTransactionAsync (AddTransactionViewModel model, bool created, string rootPath, string id, string userId);

        TransactionViewModel GetTransactionInfo (string id);

        Task DeleteTransactionAsync (string id);
    }
}
