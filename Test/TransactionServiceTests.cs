using Core.Contracts;
using Core.Models.Transactions;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class TransactionServiceTests
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
                    .AddSingleton<ITransactionService, TransactionService>()
                    .AddSingleton<ICommonService, CommonService>()
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
                Assert.CatchAsync<Exception>(async () => await service!.AddTransactionAsync(model, created, "rootPath", "userId", "Krasi"), "Transaction is null.");

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
                    
                };
                bool created = false;
                var service = serviceProvider.GetService<ITransactionService>();               

                Assert.IsTrue(await service!.AddTransactionAsync(model, created, "rootPath", "userId", "Krasi"));   
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

                await repo.AddAsync(transaction);
                await repo.SaveChangesAsync();
            }
        }

    }
}
