using Core.Models.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        EditTransactionViewModel GetTransactionForEdit (string transactionId);

        Task<bool> EditTransactionAsync (EditTransactionViewModel model, bool edited, string rootPath, string id, string userId);

        string GetUserIdByName (string username);

        ICollection<CategoryViewModel> GetCategories ();

        IEnumerable<AllCategoryTransactionsViewModel> GetCategoryTransactions (int id, int itemsPerPage, string categoryId);

        int GetCategoryTransactionsCount (string categoryId);

        string GetCategoryName (string categoryId);
        decimal GetTotalAmounthTransactions (string categoryId);
    }
}
