using Core.Contracts;
using Core.Models.File;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using System.Globalization;

namespace Core.Services
{
    public class FileService : IFileService
    {
        private readonly IApplicatioDbRepository repo;

        public FileService (IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public IEnumerable<AllFilesViewModel> GetAllFiles (int page, int itemsPerPage)
        {
            var countFile = GetFilesCount();

            if (itemsPerPage > countFile)
            {
                itemsPerPage = countFile;
            }

            return repo.All<SubmittedFile>()
            .Where(e => e.IsDeleted == false)
            .Where(e => e.IsImage == false)
            .OrderBy(e => e.DateToAdd)
            .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
            .Select(e => new AllFilesViewModel()
            {
                Id = e.Id,
                TransactionName = e.TransactionName,
                TransactionId = repo.AllReadonly<Transaction>().Where(t => t.Name == e.TransactionName).Select(t => t.Id).FirstOrDefault(),
                DateToAdd = e.DateToAdd.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                FileExtension = e.Extension,
                Name = e.Id.ToString(),
            })
            .ToList();
        }

        public string GetFileNameById (string fileId)
        {
            var currentfile = repo.AllReadonly<SubmittedFile>().Where(i => i.Id == fileId).FirstOrDefault();

            if (currentfile != null)
            {
                var file = $"{fileId}.{currentfile.Extension}";
                return file;
            }
            return null;
        }

        public int GetFilesCount ()
        {
            return repo.AllReadonly<SubmittedFile>().Where(e => e.IsDeleted == false)
            .Where(e => e.IsImage == false).Count();
        }
    }
}
