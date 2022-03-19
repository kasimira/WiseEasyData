using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;

namespace Core.Services
{
    public class IndexAppService : IIndexAppService
    {
        private readonly IApplicatioDbRepository repo;
        public IndexAppService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }
        public IndexAppViewModel GetInfo()
        {
            var employeeCount = repo.All<Employee>().Count();

            var clientCount = repo.All<Client>().Count();

            var constructionSiteCount = repo.All<ConstructionSite>().Count();

            var totalSalary = repo.All<Employee>().SelectMany(e => e.Salaries).Sum(e => e.TotalAmount);

            var employeeActive = repo.All<Employee>().Select(e => e.Status.Equals("Active"));

            var countEmployeeActive = employeeActive.Count();

            var employeeInactive = repo.All<Employee>().Select(e => e.Status.ToString() == "Active").Count();

            var employeeFemale = repo.All<Employee>().Select(e => e.Gender.ToString().Equals("Female")).ToArray();
            var count = employeeFemale.Count();

            var employeeMale = repo.All<Employee>().Where(e => e.Gender.Equals("Male")).Count();

            var totalCosts = repo.All<Cost>().Select(c => c.Value).Sum();

            var info = new IndexAppViewModel()
            {
                EmployeeCount = employeeCount,
                EmployeeActive = countEmployeeActive,
                EmployeeInactive = employeeInactive,
                TotalCost = totalCosts,
                TotalSalary = totalSalary,
                EmployeeFemale = count,
                EmployeeMele = employeeMale,
                ConstructionSiteCount = constructionSiteCount,
                ClientCount = clientCount,

            };
            return info;
        }
    }
}

//
// int EmployeeCount { get; set; }
//
// int EmployeeHours { get; set; }
//
// int HoursPorMounth { get; set; }
//
// int UsersCount { get; set; }
//
// double TotalCost { get; set; }
//
// double TotalSalary { get; set; }
//
// int EmployeeMele { get; set; }
//
// int EmployeeFemale { get; set; }
//
// int EmployeeActive { get; set; }
//
// int EmployeeInactive { get; set; }