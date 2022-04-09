using Core.Contracts;
using Core.Models.Transactions;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class TransactionServiceTests
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
                .AddSingleton<ApplicationUser>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IEmployeeService, EmployeeService>()
                .AddSingleton<ITransactionService, TransactionService>()
                .AddSingleton<ICommonService, CommonService>()
                .AddSingleton<IFileService, FileService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [Test]
        public void ThrowWhenTransactionIsNull ()
        {
            var model = new AddTransactionViewModel()
            {
            };
            bool created = false;
            var service = serviceProvider.GetService<ITransactionService>();
            Assert.CatchAsync<Exception>(async () => await service!.AddTransactionAsync(model, created, "rootPath", "userId", "d599d6c0-5a3a-4946-90da-6418e909697f"), "Transaction is null.");

        }

        [Test]
        public async Task CheskIfTransactionIsAddedCorrectly ()
        {
            var model = new AddTransactionViewModel()
            {
                Id = "111111111",
                Name = "Transaction Name",
                Date = DateTime.UtcNow,
                Amount = 75,
                Currency = "EURO",
                TransactionType = "Expense",
                Description = null,
                CategoryTransactions = "Fuell",  
                File = null,
                IsDeleted = false,
                Categories = null
            };
            bool created = false;
            var service = serviceProvider.GetService<ITransactionService>();

            Assert.IsTrue(await service!.AddTransactionAsync(model, created, "rootPath", "userId", "UserId"));
        }

        [Test]
        public void ThrowWhenTransactionInfoIsNull ()
        {
            var service = serviceProvider.GetService<ITransactionService>();

            Assert.CatchAsync<Exception>(async () => service!.GetTransactionInfo(null!), "Transaction is null.");
        }

        [Test]
        public void CheckTransactionInfoIsReturn ()
        {
            var service = serviceProvider.GetService<ITransactionService>();

            Assert.DoesNotThrowAsync(async () => service!.GetTransactionInfo("123456789"));
        }

        [Test]
        public void CheckIsCategoryAddedCorrectly ()
        {
            var service = serviceProvider.GetService<ITransactionService>();

            var model = new AddCategoryTransactionViewModel()
            {
                Name = "New Category Name",
            };

            Assert.DoesNotThrowAsync(async () => await service!.AddCategory(model));
        }

        [Test]
        public void ThrowIsCategoryAlredyExite ()
        {
            var service = serviceProvider.GetService<ITransactionService>();

            var model = new AddCategoryTransactionViewModel()
            {
                Name = "Fuell",
            };

            Assert.CatchAsync<ArgumentException>(async () => await service!.AddCategory(model), "The category already exists.");
        }

        [Test]
        public void ThrowAnErrorWhenSomeoneWhoDidNotCreateTheTransactionTriesToChangeIt ()
        {
            var service = serviceProvider.GetService<ITransactionService>();
            bool edited = false;

            var model = new EditTransactionViewModel()
            {
                Name = "Transaction Name",
                Date = DateTime.UtcNow,
                Amount = 75,
                Currency = "EURO",
                TransactionType = "Expense",
                Description = null,
                CategoryTransactions = "Fuell",
                File = null
            };

            Assert.CatchAsync<ArgumentException>(async () => await service!.EditTransactionAsync(model, edited, "rootPath", "123456789", "userId"), "You are not authorized to perform this action.");
        }

        [Test]
        public void ThrowAnErrorWhenTheCalledForCorrectionTransactionIsNotFound ()
        {
            var service = serviceProvider.GetService<ITransactionService>();

            Assert.CatchAsync<Exception>(async () => service!.GetTransactionForEdit("incorrectId"), "Transaction is null.");
        }

        [Test]
        public void CheckIfTheTransactionHasBeenSuccessfullyModified ()
        {
            var service = serviceProvider.GetService<ITransactionService>();
            bool edited = false;

            var model = new EditTransactionViewModel()
            {
                Name = "Transaction Changed Name",
                Date = DateTime.UtcNow,
                Amount = 75,
                Currency = "EURO",
                TransactionType = "Expense",
                Description = null,
                CategoryTransactions = "Fuell",
                File = null
            };
            service!.EditTransactionAsync(model, edited, "rootPath", "123456789", "123456789");
            var trasactionName = service.GetTransactionInfo("123456789").Name;

            Assert.AreEqual(trasactionName, "Transaction Changed Name");
        }

        [TearDown]
        public void TearDown ()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync (IApplicatioDbRepository repo)
        {
            var caterogy = new CategoryTransactions()
            {
                Id = "1234",
                Name = "Fuell",
                IsDeleted = false,
                Transactions = new List<Transaction>()
            };

            var transaction = new Transaction()
            {
                Id = "123456789",
                Name = "Transaction Name",
                CategoryId = "1234",
                Category = caterogy,
                Date = DateTime.UtcNow,
                Amount = 75,
                Currency = Currency.EURO,
                TransactionType = TransactionType.Expense,
                FileId = null,
                Description = null,
                CreatorName = "Krasimira",
                DataToAdd = DateTime.UtcNow,
                CreatorId = "123456789",
                IsDeleted = false,
            };

            var user = new ApplicationUser()
            {
                UserName = "username",
                Id = "UserId",
                Email = "user@user.com",
                FirstName = "FirstName",
                LastName = "LastName",
                JoinedDate = DateTime.UtcNow,

            };
            await repo.AddAsync(user);
            await repo.AddAsync(caterogy);
            await repo.AddAsync(transaction);
            await repo.SaveChangesAsync();
        }
    }
}
