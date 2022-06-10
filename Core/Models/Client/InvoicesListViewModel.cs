namespace Core.Models.Client
{
    public class InvoicesListViewModel
    {
        public IEnumerable<AllInvoicesViewModel> Invoices { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)InvoicesCount / ItemsPerPage);

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int InvoicesCount { get; set; }

        public int ItemsPerPage { get; set; }

        public string ClientName { get; set; }  

        public decimal TotalAmount { get; set; }
    }
}
