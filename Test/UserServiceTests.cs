using Core.Contracts;
using Core.Services;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Test
{
    public class UserServiceTests
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        //private Mock<UserManager<ApplicationUser>> userManager;

        [SetUp]
        public async Task Setup ()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<UserManager<ApplicationUser>>()
                .BuildServiceProvider();
           

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [TearDown]
        public void TearDown ()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync (IApplicatioDbRepository repo)
        {


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
            await repo.SaveChangesAsync();
        }
    }
}

