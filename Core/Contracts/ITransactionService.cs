using Core.Models.Transactions;
using System.Web.Mvc;

namespace Core.Contracts
{
    public interface ITransactionService
    {
        IEnumerable<AllTransactionsViewModel> GetTransactions (int page, int itemsPerPage);

        int GetCountTransactions ();

        Task<bool> AddTransactionAsync (AddTransactionViewModel model, bool created, string rootPath, string id, string userId);

        TransactionViewModel GetTransactionInfo (string id);

        Task DeleteTransactionAsync (string id);

        //Task AddCategory (AddCategoryTransactionViewModel model);

        IEnumerable<SelectListItem> GetAllCategories ();

        Task AddCategory (AddCategoryTransactionViewModel model);
    }
}
