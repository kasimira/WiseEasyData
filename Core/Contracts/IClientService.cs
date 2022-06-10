using Core.Models.Client;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IClientService
    {
        int GetCount ();

        Task DeleteAsync (string clientId);

        Task ChangeStatusAsync (string clientId);

        public Client GetClient (string clientId);

        ClientProfileViewModel GetClientProfil (string id);

        DeleteClientViewModel GetClientForDelete (string id);

        EditClientViewModel GetClientInfo<T> (string clientId);

        IEnumerable<AllClientsViewModel> GetClients (int page, int itemsPerPage);

        Task EditClientAsync (EditClientViewModel model, string clientId);       

        Task<bool> AddClientAsync (AddClientViewModel model, bool created);
        int GetCountInvoices (string clientId);
        decimal GetTotalAmountInvoices (string clientId);
        IEnumerable<AllInvoicesViewModel> GetInvoices (int id, int itemsPerPage, string clientId, int invoicesCount);
        string GetClientName (string clientId);
    }
}
