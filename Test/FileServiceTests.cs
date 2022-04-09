using Core.Contracts;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Test
{
    public class FileServiceTests
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
                .AddSingleton<IFileService, FileService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo!);
        }

        [Test]
        public void CheckThatAfterDeletionIsDeletedIsEqualATrue ()
        {
            var service = serviceProvider.GetService<IFileService>();
            service!.ChangeFileIsDeletedTrue("fileId1");
            var file = service.GetFileById("fileId1");
            Assert.IsTrue(file.IsDeleted);
        }

        [Test]
        public void CheckThatTheNameFileIsCorrect ()
        {
            var service = serviceProvider.GetService<IFileService>();
            var file = service!.GetFileNameById("fileId1");

            Assert.AreEqual(file, "fileId1.jpg");
        }

        [Test]
        public void ReturnNullWhenFileNameIsIncorrect ()
        {
            var service = serviceProvider.GetService<IFileService>();
            var file = service!.GetFileNameById("noFile");
            Assert.IsNull(file);
        }

        [Test]
        public void CheckThatTheCountFileIsCorrect ()
        {
            var service = serviceProvider.GetService<IFileService>();
            var filesCount = service!.GetFilesCount();

            Assert.AreEqual(2, filesCount);
        }

        [Test]
        public void CheckThatNumberFilesIsCorrect ()
        {
            var service = serviceProvider.GetService<IFileService>();
            var files = service!.GetAllFiles(1, 2);
            var countFiles = 0;
            foreach (var file in files)
            {
                countFiles++;
            }

            Assert.AreEqual(2, countFiles);
        }

        [TearDown]
        public void TearDown ()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync (IApplicatioDbRepository repo)
        {

            var dbFile1 = new SubmittedFile()
            {
                Id = "fileId1",
                OwnerId = "userId",
                Extension = "jpg",
                IsImage = false,
                TransactionName = "transactionName",
                IsDeleted = false
            };

            var dbFile2 = new SubmittedFile()
            {
                Id = "fileId2",
                OwnerId = "userId2",
                Extension = "jpg",
                IsImage = false,
                TransactionName = "transactionName2",
                IsDeleted = false
            };

            await repo.AddAsync(dbFile1);
            await repo.AddAsync(dbFile2);
            await repo.SaveChangesAsync();
        }
    }
}
