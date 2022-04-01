namespace Core.Models.User
{
    public class UsersListViewModel
    {
        public IEnumerable<AllUsersViewModel> Users { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)UserCount / ItemsPerPage);

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int UserCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
