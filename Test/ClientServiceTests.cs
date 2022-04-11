using Core.Contracts;
using Core.Models.Client;
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
    public class ClientServiceTests
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
                .AddSingleton<IClientService, ClientService>()
                .AddSingleton<ICommonService, CommonService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [Test]
        public void ThrowWhenClientIsNull ()
        {
            var model = new AddClientViewModel()
            {

            };
            bool created = false;

            var service = serviceProvider.GetService<IClientService>();
            Assert.CatchAsync<Exception>(async () => await service!.AddClientAsync(model, created), "Client is null.");

        }

        [Test]
        public void CheckThatTheNumberOfClientsIsCorrect ()
        {
            var service = serviceProvider.GetService<IClientService>();
            Assert.AreEqual(1, service!.GetCount());

        }

        [Test]
        public void CheckThatTheNumberOfListClientsIsCorrect ()
        {
            var service = serviceProvider.GetService<IClientService>();
            var count = 0;
            var listClient = service!.GetClients(1, 3);
            foreach (var item in listClient)
            {
                count++;
            }
            Assert.AreEqual(1, count);
        }

        [Test]
        public void CheckThatTheClientForDeleteIsTheSame ()
        {
            var service = serviceProvider.GetService<IClientService>();
            Assert.DoesNotThrowAsync(async () => service!.GetClientForDelete("123456789"));
        }

        [Test]
        public void CheckIsreturnCorrectlyTheClentPtofil ()
        {
            var service = serviceProvider.GetService<IClientService>();
            Assert.DoesNotThrowAsync(async () => service!.GetClientProfil("123456789"));
        }

        [Test]
        public void CheckIsreturnCorrectlyTheClentInfo ()
        {
            var service = serviceProvider.GetService<IClientService>();
            Assert.DoesNotThrowAsync(async () => service!.GetClientInfo<EditClientViewModel>("123456789"));
        }

        [Test]
        public void ThrowWhenTheClientForDeleteNoExist ()
        {
            var service = serviceProvider.GetService<IClientService>();
            Assert.ThrowsAsync<NullReferenceException>(() => { service!.GetClientForDelete("0000000000"); return Task.CompletedTask; });
        }

        [Test]
        public async Task CheckIfTheStatusChangeCorrectly ()
        {
            var service = serviceProvider.GetService<IClientService>();
            var client = service!.GetClient("123456789");
            await service.ChangeStatusAsync(client.Id);
            Assert.AreEqual(Status.Inactive, client.Status);
        }

        [Test]
        public async Task CheckIfTheClientIsDeletedCorrectly ()
        {
            var service = serviceProvider.GetService<IClientService>();
            await service!.DeleteAsync("123456789");

            Assert.AreEqual(service.GetCount(), 0);
        }

        [Test]
        public async Task CheckIfTheClientInfoCahgeCorrectly ()
        {
            var model = new EditClientViewModel()
            {
                Name = "Client new Name",
                Address = "Address",
                PhoneNumber = "0867585858",
                Email = "client@client.com",
                Website = "clent.com",
                PostalCode = "4000",
                Status = "Active",
                HireDate = new DateTime(2022, 3, 16),
                City = "City",
                CeoName = "Ceo Name",
                VatNumber = "vatnumber",
                ReleaseDate = null,
                MobilePhone=null,
                Country = "Country"

            };

            var service = serviceProvider.GetService<IClientService>();
            await service!.EditClientAsync(model, "123456789");
            var newName = service.GetClient("123456789");
            Assert.AreEqual(newName.Name, "Client new Name");
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
                Name = "Country",
                Cities = new List<City>()
            };
            var city = new City()
            {
                Name = "City",
                Country = country,
            };

            var client = new Client()
            {
                Id = "123456789",
                Name = "Client Name",
                Address = "Address",
                PhoneNumber = "0867585858",
                Email = "client@client.com",
                Website = "clent.com",
                PostalCode = "4000",
                Status = Status.Active,
                HireDate = new DateTime(2022, 3, 16),
                IsDeleted = false,
                City = city,
                CeoName = "Ceo Name",
                VatNumber = "vatnumber"

            };

            await repo.AddAsync(client);
            await repo.SaveChangesAsync();
        }
    }
}
