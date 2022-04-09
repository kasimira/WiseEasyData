using Core.Contracts;
using Core.Models.Salaries;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class SalaryServiceTests
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup ()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<ISalaryService, SalaryService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [Test]
        public void ThrowWhenTrieToAddSalaryEmployeeIsNull ()
        {
            var model = new AddSalaryViewModel()
            {
            };
            bool created = false;
            var service = serviceProvider.GetService<ISalaryService>();
            Assert.CatchAsync<Exception>(async () => await service!.AddSalaryAsync(model, created), "Employee is null.");
        }

       //[Test]
       //public void ThrowWhenTrieToAddSalaryModelIsIncorrect ()
       //{
       //    var model = new AddSalaryViewModel()
       //    {
       //        EmployeeName = "123456789",
       //    };
       //    bool created = false;
       //    var service = serviceProvider.GetService<ISalaryService>();
       //    Assert.CatchAsync<Exception>(async () => await service!.AddSalaryAsync(model, created));
       //}

        [Test]
        public void CheckThatTheSalaryIsAddedCorrectly ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            bool created = false;

            var model = new AddSalaryViewModel()
            {
                Salary = (decimal)1900,
                InAdvance = (decimal)200,
                TotalAmount = (decimal)2100,
                FromDate = new DateTime(2022, 2, 1),
                ToDate = new DateTime(2022, 2, 28),
                HoursWorked = 200,
                EmployeeName = "123456789",
                Note = null,
            };

            Assert.DoesNotThrowAsync(async () => await service!.AddSalaryAsync(model, created));
        }

        [Test]
        public void CheckThatTheNumberOfSalariesIsCorrect ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            Assert.AreEqual(2, service!.GetCountSalaries());
        }

        [Test]
        public void CheckThatTheEmployeeSalariesCountIsCorrect ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            var employeeSalaries = service!.GetSalariesEmployee(1, 6, "123456789");
            var count = 0;
            foreach (var item in employeeSalaries)
            {
                count++;
            }
            Assert.AreEqual(count, 2);
        }

        [Test]
        public void CheckThatSalaryIsTheCorrect ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            var salary = service!.GetSalaryInfo("1234");
            Assert.AreEqual(2000, salary.TotalAmount);
        }

        [Test]
        public void CheckThatSalaryIsEditedCorrectly ()
        {
            var service = serviceProvider.GetService<ISalaryService>();

            var model = new EditSalaryViewModel()
            {
                Salary = (decimal)1900,
                InAdvance = (decimal)200,
                TotalAmount = (decimal)2100,
                FromDate = new DateTime(2022, 2, 1),
                ToDate = new DateTime(2022, 2, 28),
                HoursWorked = 200,
                EmployeeName = "123456789",
                Note = null,            
            };

            service!.EditSalaryAsync(model, "1234");
            var salary = service.GetSalaryInfo("1234");

            Assert.AreEqual(salary.HoursWorked, 200);
        }

        [Test]
        public void CheckThatSalaryIsDeleted ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            Assert.DoesNotThrowAsync(async () => await service!.DeleteSalaryAsync("1234"));
        }

        [Test]
        public void CheckThatSalaryDoesNotExistWhenDeleted ()
        {
            var service = serviceProvider.GetService<ISalaryService>();
            service!.DeleteSalaryAsync("1234");
            var countSalaries = service.GetCountSalaries();
            Assert.AreEqual(1, countSalaries);
        }


        [TearDown]
        public void TearDown ()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync (IApplicatioDbRepository repo)
        {
            var salary = new Salary()
            {
                Id = "1234",
                FromDate = new DateTime(2022, 2, 1),
                ToDate = new DateTime(2022, 2, 28),
                SalaryAmount = 1900,
                TotalAmount = 2000,
                InAdvance = 100,
                HourlySalary = 10,
                HoursWorked = 250,
                EmployeeId = "123456789"
            };

            var salary2 = new Salary()
            {
                Id = "123456",
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow,
                SalaryAmount = 1900,
                TotalAmount = 2000,
                InAdvance = 100,
                HourlySalary = 10,
                HoursWorked = 250,
                EmployeeId = "123456789"
            };

            var country = new Country()
            {
                Name = "Bulgaria",
                Cities = new List<City>()
            };
            var city = new City()
            {
                Name = "Pleven",
                Country = country,
                Employees = new List<Employee>()
            };

            var department = new Department()
            {
                Name = "Department",
                Employees = new List<Employee>()
            };

            var employee = new Employee()
            {
                Id = "123456789",
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                LastName = "LastName",
                Nationality = "Bulgarian",
                Address = "Doiran 24",
                DateOfBirth = new DateTime(1990, 9, 16),
                HourlySalary = (decimal)15,
                PhoneNumber = "0867585858",
                Email = "pesho@pesho.com",
                Position = "mechanic",
                PostalCode = "7000",
                Gender = Gender.Male,
                Grade = Grade.Primary,
                Status = Status.Active,
                HireDate = new DateTime(2022, 3, 16),
                CreatorName = "Krasi",
                IsDeleted = false,
                City = city,
                Department = department,
                ImageId = null,
            };

            var employee2 = new Employee()
            {
                Id = "123456",
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                LastName = "LastName",
                Nationality = "Bulgarian",
                Address = "Doiran 24",
                DateOfBirth = new DateTime(1990, 9, 16),
                HourlySalary = (decimal)15,
                PhoneNumber = "0867585858",
                Email = "pesho@pesho.com",
                Position = "mechanic",
                PostalCode = "7000",
                Gender = Gender.Male,
                Grade = Grade.Primary,
                Status = Status.Active,
                HireDate = new DateTime(2022, 3, 16),
                CreatorName = "Krasi",
                IsDeleted = false,
                City = city,
                Department = department,
                ImageId = null,
            };

            await repo.AddAsync(employee);
            await repo.AddAsync(employee2);
            await repo.AddAsync(salary);
            await repo.AddAsync(salary2);
            await repo.SaveChangesAsync();
        }
    }
}
