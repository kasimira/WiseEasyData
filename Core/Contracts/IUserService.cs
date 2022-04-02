using Core.Models.User;

namespace Core.Contracts
{
    public interface IUserService
    {
        IEnumerable<AllUsersViewModel> GetUsers (int page, int itemsPerPage);

        Task<EditUserViewModel> GetUserForEdit (string id);

        Task<bool> EditUser (EditUserViewModel model);

        int GetCount ();

        Task DeleteUserAsync (string id);

        public DeleteUserViewModel GetUserForDelete (string id);
    }
}
