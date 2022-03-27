namespace Core.Models.File
{
    public class FileListViewModel
    {
        public IEnumerable<AllFilesViewModel> Files { get; set; }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)FileCount / ItemsPerPage);

        public int PreviousPageNumber => PageNumber - 1;

        public int NextPageNumber => PageNumber + 1;

        public int FileCount { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
