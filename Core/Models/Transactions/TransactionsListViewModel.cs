namespace Core.Models.Transactions
{
    public class TransactionsListViewModel
    {
        public IEnumerable<AllTransactionsViewModel> Transactions { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)TransactionsCount / ItemsPerPage);

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int TransactionsCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
