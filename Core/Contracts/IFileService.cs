using Core.Models.File;
using Core.Models.Transactions;
using Infrastructure.Data;

namespace Core.Contracts
{
    public interface IFileService
    {
        string GetFileNameById (string fileId);

        IEnumerable<AllFilesViewModel> GetAllFiles (int page, int itemsPerPage);

        int GetFilesCount ();

        Task<SubmittedFile> CreateFile (EditTransactionViewModel model, string rootPath, string userId);

        SubmittedFile GetFileById (string fileId);

        Task DeleteFile (string fullPath, SubmittedFile oldFile);

        void ChangeFileIsDeletedTrue (string fileId);
    }
}
