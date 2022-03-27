using Core.Contracts;
using Core.Models;
using Core.Models.Employee;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Core.Services
{
    public class IndexAppService : IIndexAppService
    {
        private readonly IApplicatioDbRepository repo;
        public IndexAppService (IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public IEnumerable<DataPoint> GetDataPoint ()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            Dictionary<string, int> ageInfo = new Dictionary<string, int>();

            var employees = repo.AllReadonly<Employee>().Where(e => e.IsDeleted == false).Select(e => e.DateOfBirth.Year).ToList();
            var employeesCount = employees.Count();

            if (employeesCount == 0)
            {
                dataPoints.Add(new DataPoint("No employees Added", 0));
                return dataPoints;
            }

            var age25 = 0;
            var age30 = 0;
            var age35 = 0;
            var age40 = 0;
            var age45 = 0;
            var age50 = 0;
            var age55 = 0;
            var age60 = 0;
            var age65 = 0;
            var ageMore65 = 0;

            var dataNow = DateTime.Now.Year;

            foreach (var item in employees)
            {
                var yearEmployee = item;
                var age = dataNow - yearEmployee;

                switch (age)
                {
                    case < 25:
                        age25++;
                        break;
                    case < 30:
                        age30++;
                        break;
                    case < 35:
                        age35++;
                        break;
                    case < 40:
                        age40++;
                        break;
                    case < 45:
                        age45++;
                        break;
                    case < 50:
                        age50++;
                        break;
                    case < 55:
                        age55++;
                        break;
                    case < 60:
                        age60++;
                        break;
                    case < 65:
                        age65++;
                        break;
                    case > 65:
                        ageMore65++;
                        break;
                    default:
                        break;
                }
            }

            var porcent25 = age25 * 100 / employeesCount;
            var porcent30 = age30 * 100 / employeesCount;
            var porcent35 = age35 * 100 / employeesCount;
            var porcent40 = age40 * 100 / employeesCount;
            var porcent45 = age45 * 100 / employeesCount;
            var porcent50 = age50 * 100 / employeesCount;
            var porcent55 = age55 * 100 / employeesCount;
            var porcent60 = age60 * 100 / employeesCount;
            var porcent65 = age65 * 100 / employeesCount;
            var porcentMore65 = ageMore65 * 100 / employeesCount;

            ageInfo.Add("Up to 25", porcent25);
            ageInfo.Add("26-30", porcent30);
            ageInfo.Add("31-35", porcent35);
            ageInfo.Add("36-40", porcent40);
            ageInfo.Add("41-45", porcent45);
            ageInfo.Add("46-50", porcent50);
            ageInfo.Add("51-55", porcent55);
            ageInfo.Add("56-60", porcent60);
            ageInfo.Add("61-65", porcent65);
            ageInfo.Add("Over 65", porcentMore65);

            foreach (var item in ageInfo)
            {
                if (item.Value > 0)
                {
                    dataPoints.Add(new DataPoint(item.Key, item.Value));
                }
            }

            return dataPoints;
        }

        public IndexAppViewModel GetInfo ()
        {
            var employeeCount = repo.AllReadonly<Employee>()
                .Where(e => e.IsDeleted == false)
                .Count();

            var clientCount = repo.AllReadonly<Client>()
                .Where(c => c.IsDeleted == false)
                .Count();

            var totalSalary = repo.AllReadonly<Employee>().Where(e => e.IsDeleted == false)
                .SelectMany(e => e.Salaries)
                .Where(d => d.FromDate.Month == DateTime.Now.Month)
                .Sum(e => e.TotalAmount);

            var totalExpenses = repo.AllReadonly<Transaction>()
                .Where(c => c.IsDeleted == false)
                .Where(c => c.TransactionType == TransactionType.Expense)
                .Sum(c => c.Amount);

            var totalExpensesMonth = repo.AllReadonly<Transaction>()
                .Where(c => c.IsDeleted == false)
                .Where(c => c.TransactionType == TransactionType.Expense)
                .Where(d => d.Date.Month == DateTime.Now.Month)
                .Sum(c => c.Amount);

            var totalIncomes = repo.AllReadonly<Transaction>()
                .Where(c => c.IsDeleted == false)
                .Where(c => c.TransactionType == TransactionType.Income)
                .Sum(c => c.Amount);

            var totalIncomesMonth = repo.AllReadonly<Transaction>()
                .Where(c => c.IsDeleted == false)
                .Where(c => c.TransactionType == TransactionType.Income)
                .Where(d => d.Date.Month == DateTime.Now.Month)
                .Sum(c => c.Amount);

            var totalEmployees = repo.AllReadonly<Employee>()
                .Where(e => e.IsDeleted == false)
                .Count();

            var ifAnyTransaction = repo.AllReadonly<Transaction>()
                .Where(t => t.IsDeleted == false)
                .Count();

            List<ExpenseViewModel> expeseCategories = new List<ExpenseViewModel>();

            List<LastTransactionsViewModel> lastTransaction = new List<LastTransactionsViewModel>();

            List<LastAddedEmployeesViewModel> lastEmployees = new List<LastAddedEmployeesViewModel>();

            if (totalExpensesMonth > 0)
            {
                expeseCategories = repo.All<Transaction>()
                .Include("Category")
                .Where(c => c.Category.Id == c.CategoryId)
                .Where(t => t.TransactionType == TransactionType.Expense)
                .Where(t => t.IsDeleted == false)
                .Where(t => t.Date.Month == DateTime.Now.Month)
                .ToList()
                .GroupBy(c => c.Category.Name)
                .Select(c => new ExpenseViewModel
                {
                    Amount = c.Sum(b => b.Amount),
                    CategoryName = c.Key
                })
                .OrderBy(c => c.Amount)
                .ToList();
            }

            if (ifAnyTransaction > 0)
            {
                lastTransaction = repo.AllReadonly<Transaction>()
               .Where(t => t.IsDeleted == false)
               .OrderByDescending(t => t.DataToAdd)
               .Take(10)
               .Select(t => new LastTransactionsViewModel
               {
                   Name = t.Name,
                   Date = t.DataToAdd.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                   Type = t.TransactionType,
               })
               .ToList();
            }
            else
            {
                lastTransaction.Add(new LastTransactionsViewModel
                {
                    Name = "No transactions added ",
                    Date = "No date",
                });
            }

            if (employeeCount > 0)
            {
                lastEmployees = repo.AllReadonly<Employee>()
                .Where(e => e.IsDeleted == false)
                .OrderByDescending(e => e.DateToAdd)
                .Take(5)
                .Select(e => new LastAddedEmployeesViewModel()
                {
                    Name = e.FirstName + " " + e.MiddleName + " " + e.LastName
                })
                .ToList();
            }
            else
            {
                lastEmployees.Add(new LastAddedEmployeesViewModel()
                {
                    Name = "No employees added ",
                });
            }

            var info = new IndexAppViewModel()
            {
                EmployeeCount = employeeCount,
                TotalExpenses = totalExpenses,
                TotalIncomes = totalIncomes,
                TotalSalary = totalSalary,
                ClientCount = clientCount,
                Expenses = expeseCategories,
                TotalExpensesMonth = totalExpensesMonth,
                TotalIncomesMonth = totalIncomesMonth,
                LastTransactions = lastTransaction,
                LastEmployees = lastEmployees,
            };

            return info;
        }
    }
}



