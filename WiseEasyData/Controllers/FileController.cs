using Core.Contracts;
using Core.Models.File;
using Microsoft.AspNetCore.Mvc;


namespace WiseEasyData.Controllers
{
    public class FileController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IFileService fileService;

        public FileController (IWebHostEnvironment _webHostEnvironment, IFileService _fileService)
        {
            webHostEnvironment = _webHostEnvironment;
            fileService = _fileService;
        }

        public FileResult DownloadFile (string fileId)
        {
            var fileName = fileService.GetFileNameById(fileId);

            try
            {
                //Build the File Path.
                string path = Path.Combine(webHostEnvironment.WebRootPath, "files/transaction/") + fileName;

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                //Send the File to Download.
                return File(bytes, "application/octet-stream", fileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult All (int id = 1)
        {
            //Fetch all files in the Folder (Directory).
            string[] filePaths = Directory.GetFiles(Path.Combine(webHostEnvironment.WebRootPath, "files/transaction/"));

            const int ItemsPerPage = 6;
            var viewModel = new FileListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                FileCount = fileService.GetFilesCount(),
                Files = fileService.GetAllFiles(id, ItemsPerPage),
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ViewPDF (string Id)
        {
            string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"500px\" height=\"300px\">";
            embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
            embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            embed += "</object>";
            TempData["Embed"] = string.Format(embed, ($"~/files/transaction/{Id}.pdf"));

            return RedirectToAction("FileView");
        }

        public ActionResult FileView ()
        {
            return View();
        }
    }
}
