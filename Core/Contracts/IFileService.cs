using Core.Models.File;

namespace Core.Contracts
{
    public interface IFileService
    {
        string GetFileNameById (string fileId);

        IEnumerable<AllFilesViewModel> GetAllFiles (int page, int itemsPerPage);

        int GetFilesCount ();
    }
}
