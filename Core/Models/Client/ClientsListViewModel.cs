namespace Core.Models.Client
{
    public class ClientsListViewModel
    {
        public IEnumerable<AllClientsViewModel> Clients { get; set; }

        public int PageNumber { get; set; } 

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.ClientCount / this.ItemsPerPage);

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;
        
        public int ClientCount { get; set; }  
         
        public int ItemsPerPage { get; set; }
    }
}
