using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
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
            var employeeCount = repo.All<Employee>().Where(e => e.IsDeleted == false).Count();

            var clientCount = repo.All<Client>().Where(c => c.IsDeleted == false).Count();

            var constructionSiteCount = repo.All<ConstructionSite>().Where(c => c.IsDeleted == false).Count();

            var totalSalary = repo.All<Employee>().Where(e => e.IsDeleted == false).SelectMany(e => e.Salaries).Sum(e => e.TotalAmount);

            var employeeActive = repo.All<Employee>().Where(e => e.IsDeleted == false).Select(e => e.Status == Status.Active).Count();

            var employeeInactive = repo.All<Employee>().Where(e => e.IsDeleted == false).Select(e => e.Status == Status.Inactive).Count();

            var employeeFemale = repo.All<Employee>().Where(e => e.IsDeleted == false).Select(e => e.Gender == Gender.Female).Count();

            var employeeMale = repo.All<Employee>().Where(e => e.IsDeleted == false).Select(e => e.Gender == Gender.Male).Count();

            var totalCosts = repo.All<Cost>().Where(c => c.IsDeleted == false).Select(c => c.Value).Sum();

            var info = new IndexAppViewModel()
            {
                EmployeeCount = employeeCount,
                EmployeeInactive = employeeInactive,
                TotalCost = totalCosts,
                TotalSalary = totalSalary,
                EmployeeFemale = employeeFemale,
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
