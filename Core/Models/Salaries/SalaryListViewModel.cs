namespace Core.Models.Salaries
{
    public class SalaryListViewModel
    {
        public IEnumerable<AllSalariesViewModel> Salaries { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)SalariesCount / ItemsPerPage);

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int SalariesCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
