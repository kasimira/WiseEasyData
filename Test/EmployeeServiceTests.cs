using Core.Contracts;
using Core.Models.Employee;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class EmployeeServiceTests
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
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<ICommonService, CommonService>()
                .AddSingleton<IFileService, FileService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [Test]
        public void ThrowWhenEmployeeIsNull ()
        {
            var model = new AddEmployeeViewModel()
            {

            };
            bool created = false;

            var service = serviceProvider.GetService<IEmployeeService>();
            Assert.CatchAsync<Exception>(async () => await service!.AddEmployeeAsync(model, created, "rootPath", "userId", "Krasi"), "Employee is null.");

        }

        [Test]
        public void CheckThatTheNumberOfEmployeesIsCorrect ()
        {
            var service = serviceProvider.GetService<IEmployeeService>();
            Assert.AreEqual(1, service!.GetCount());

        }

        [Test]
        public void CheckThatTheEmployeeForDeleteIsTheSame ()
        {
            var service = serviceProvider.GetService<IEmployeeService>();
            Assert.DoesNotThrowAsync(async () => service!.GetEmployeeForDelete("123456789"));
        }

        [Test]
        public void ThrowWhenTheEmployeeForDeleteNoExist ()
        {
            var service = serviceProvider.GetService<IEmployeeService>();
            Assert.ThrowsAsync<NullReferenceException>(() => { service!.GetEmployeeForDelete("0000000000"); return Task.CompletedTask; });
        }

        [Test]
        public async Task CheckIfTheStatusChangeCorrectly ()
        {
            var service = serviceProvider.GetService<IEmployeeService>();
            var employee = service!.GetEmployee("123456789");
            await service.ChangeStatusAsync(employee.Id);
            Assert.AreEqual(Status.Inactive, employee.Status);
        }

        [Test]
        public async Task CheckIfTheEmployeeIsDeletedCorrectly ()
        {
            var service = serviceProvider.GetService<IEmployeeService>();
            var employee = service!.GetEmployee("123456789");
            await service.DeleteAsync("123456789");

            Assert.AreEqual(service.GetCount(), 0);
        }

        [Test]
        public async Task CheckIfTheEmployeeInfoCahgeCorrectly ()
        {
            var model = new EditEmployeeViewModel()
            {
                FirstName = "Gosho",
                MiddleName = "Goshev",
                LastName = "Goshev",
                Nationality = "Bulgarian",
                Address = "Doiran 24",
                DateOfBirth = new DateTime(1990, 9, 16),
                HourlySalary = (decimal)15,
                PhoneNumber = "0867585858",
                Email = "pesho@pesho.com",
                Position = "mechanic",
                PostalCode = "7000",
                Gender = "Male",
                Grade = "Primary",
                Status = "Active",
                HireDate = new DateTime(2022, 3, 16),
                City = "Pleven",
                Department = "Department",
            };

            var service = serviceProvider.GetService<IEmployeeService>();
            await service!.EditEmployeeAsync(model, "123456789", "rootPath");
            Assert.AreEqual(service.GetEmployee("123456789").FirstName, "Gosho");
        }

        [TearDown]
        public void TearDown ()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync (IApplicatioDbRepository repo)
        {
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
                FirstName = "Pesho",
                MiddleName = "Peshov",
                LastName = "Peshov",
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
            await repo.SaveChangesAsync();
        }
    }
}
