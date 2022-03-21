namespace Core.Models.Employee
{
    public class EmployeesListViewModel
    {
        public IEnumerable<AllEmployeesViewModel> Employees { get; set; }

        public int PageNumber { get; set; } 

        public bool HasPreviousPage => this.PageNumber > 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.EmployeeCount / this.ItemsPerPage);

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;
        
        public int EmployeeCount { get; set; }  
         
        public int ItemsPerPage { get; set; }

    }
}
