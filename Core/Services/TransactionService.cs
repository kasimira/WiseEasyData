using Core.Contracts;
using Core.Models.Transactions;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Identity;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Core.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly IApplicatioDbRepository repo;
        private readonly IFileService fileService;

        private string[] colorsArray = { "#a362ea", "#fc5f9b", "#0ed095", "#0d6efd", "#fd7e14", "#d63384", "#dc3545", "#ffc107", "#198754", "#20c997", "#adb5bd", "#0dcaf0", "#6f42c1" };
        private List<string> colors = new List<string>();
        private string[] iconsArray = { "cash-outline", "wallet-outline", "receipt-outline", "journal-outline", "layers-outline", "reader-outline", "calculator-outline", "card-outline", "document-text-outline", "reader-outline" };
        private List<string> icons = new List<string>();

        public TransactionService (IApplicatioDbRepository _repo, IFileService _fileService)
        {
            repo = _repo;
            fileService = _fileService;
        }

        public IEnumerable<AllTransactionsViewModel> GetTransactions (int page, int itemsPerPage)
        {
            var countTransactions = GetCountTransactions();

            if (itemsPerPage > countTransactions)
            {
                itemsPerPage = countTransactions;
            }

            return repo.AllReadonly<Transaction>()
                .Where(t => t.IsDeleted == false)
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(t => new AllTransactionsViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Date = t.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    TransactionType = t.TransactionType,
                    FileId = t.FileId!,
                    Category = t.Category.Name!,
                    CreatorName = t.CreatorName,
                })
                .ToList();
        }

        public async Task<bool> AddTransactionAsync (AddTransactionViewModel model, bool created, string rootPath, string id, string userId)
        {
            string fileId = null!;
            SubmittedFile dbFile = null!;

            if (model.File != null)
            {
                var extension = Path.GetExtension(model.File.FileName).TrimStart('.');

                dbFile = new SubmittedFile()
                {
                    OwnerId = userId,
                    Extension = extension,
                    IsImage = false,
                    TransactionName = model.Name,
                };

                fileId = dbFile.Id;

                Directory.CreateDirectory($"{rootPath}/files/transaction/");
                var physicalPath = $"{rootPath}/files/transaction/{dbFile.Id}.{extension}";

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fs);
                }
            }

            var category = GetCategory(model.CategoryTransactions);
            var client = GetClient(model.ClientName);

            if (category == null)
            {
                category = new CategoryTransactions()
                {
                    Name = model.CategoryTransactions,
                    Transactions = new List<Transaction>()
                };

                await repo.AddAsync(category);
            }

            var transaction = new Transaction()
            {
                Name = model.Name,
                CategoryId = category.Id,
                Date = model.Date,
                Amount = model.Amount,
                Currency = (Currency)Enum.Parse(typeof(Currency), model.Currency),
                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), model.TransactionType),
                FileId = fileId,
                Description = model.Description,
                CreatorId = userId,
                CreatorName = GetUserNameById(userId),
                ClientId = model.ClientName,                
            };

            if (transaction == null)
            {
                throw new Exception("Transaction is null.");
            }

            if (dbFile != null)
            {
                await repo.AddAsync(dbFile);
            }

            category.Transactions.Add(transaction);

            if (client != null)
            {
                client.Invoices.Add(transaction);
            }

            try
            {
                await repo.AddAsync(transaction);
                await repo.SaveChangesAsync();
                created = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (created);
        }

        public TransactionViewModel GetTransactionInfo (string id)
        {
            var fileId = repo.AllReadonly<Transaction>().Where(e => e.Id == id).Select(e => e.FileId).FirstOrDefault();
            var file = "";
            var imageFile = "";


            if (fileId != null)
            {
                var currentfile = fileService.GetFileById(fileId);

                file = $"{fileId}.{currentfile!.Extension}";

                var extension = currentfile.Extension;

                if (extension == "pdf" || extension == "docx" || extension == "xlsx")
                {
                    imageFile = "noFile.jpg";
                }
                else
                {
                    imageFile = file;
                }
            }
            else
            {
                file = "noFile.jpg";
                imageFile = "noFileUpload.jpg";
            }

            var transaction = repo.AllReadonly<Transaction>().Where(e => e.Id == id)
                .Select(t => new TransactionViewModel()
                {
                    Id = id,
                    Name = t.Name,
                    Description = t.Description,
                    Date = t.Date,
                    File = file,
                    ImageFile = imageFile,
                    FileId = fileId!,
                    Currency = t.Currency,
                    TransactionType = t.TransactionType,
                    DataToAdd = t.DataToAdd,
                    CreatorName = t.CreatorName,
                    Amount = t.Amount,
                    Category = t.Category.Name,
                })
                .FirstOrDefault();

            if (transaction == null)
            {

                throw new Exception("Transaction is null.");
            }

            return transaction;
        }

        public async Task DeleteTransactionAsync (string transactionId)
        {
            var transaction = GetTransactionById(transactionId);
            var category = repo.All<CategoryTransactions>().Where(c => c.Id == transaction.CategoryId).FirstOrDefault();
            category!.Transactions.Remove(transaction);

            if (transaction!.FileId != null)
            {
                fileService.ChangeFileIsDeletedTrue(transaction.FileId);
            }

            transaction.IsDeleted = true;


            await repo.SaveChangesAsync();
        }

        public IEnumerable<SelectListItem> GetAllCategories ()
        {
            var query = repo.All<CategoryTransactions>()
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList();

            return query;
        }

        public IEnumerable<SelectListItem> GetAllClients ()
        {
            var query = repo.All<Client>()
                .Where(x => x.IsDeleted == false)
                .OrderBy(x => x.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Name
                }).ToList();

            return query;
        }

        public async Task AddCategory (AddCategoryTransactionViewModel model)
        {
            var category = GetCategoryByName(model.Name);

            if (category == null)
            {
                category = new CategoryTransactions()
                {
                    Name = model.Name
                };
            }
            else
            {
                throw new ArgumentException("The category already exists.");
            }

            try
            {
                await repo.AddAsync(category);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("Category record failed.");
            }
        }

        public EditTransactionViewModel GetTransactionForEdit (string transactionId)
        {
            var transaction = GetTransactionById(transactionId);

            var transactionModel = new EditTransactionViewModel()
            {
                Name = transaction!.Name,
                Description = transaction.Description,
                Date = transaction.Date,
                Amount = transaction.Amount,
                Categories = GetAllCategories(),
            };

            if (transaction == null)
            {
                throw new Exception("Transaction is null.");
            }

            return transactionModel;
        }

        public async Task<bool> EditTransactionAsync (EditTransactionViewModel model, bool edited, string rootPath, string transactionId, string userId)
        {
            var transaction = GetTransactionById(transactionId);

            if (transaction != null)
            {
                if (userId != transaction.CreatorId)
                {
                    throw new ArgumentException("You are not authorized to perform this action.");
                }
            }

            SubmittedFile dbFile = null!;

            if (model.File != null)
            {
                if (transaction!.FileId != null)
                {
                    var OldFile = fileService.GetFileById(transaction.FileId);

                    OldFile!.IsDeleted = true;

                    var fullPath = $"{rootPath}/files/transaction/{OldFile.Id}.{OldFile.Extension}";

                    await fileService.DeleteFile(fullPath, OldFile);
                }

                dbFile = await fileService.CreateFile(model, rootPath, userId);
                transaction!.FileId = dbFile.Id;
                await repo.AddAsync(dbFile);
            }

            var category = GetCategory(model.CategoryTransactions);

            if (category == null)
            {
                category = new CategoryTransactions()
                {
                    Name = model.CategoryTransactions,
                    Transactions = new List<Transaction>()
                };

                await repo.AddAsync(category);
            }

            transaction!.Name = model.Name;
            transaction.Description = model.Description;
            transaction.TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), model.TransactionType);
            transaction.Amount = model.Amount;
            transaction.Currency = (Currency)Enum.Parse(typeof(Currency), model.Currency);
            transaction.CategoryId = category.Id;

            category.Transactions.Add(transaction);

            try
            {
                await repo.SaveChangesAsync();
                edited = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (edited);
        }

        public string GetUserIdByName (string username)
        {
            var user = repo.All<ApplicationUser>().Where(u => u.UserName == username).FirstOrDefault();
            return user!.Id;
        }
        public string GetUserNameById (string userId)
        {
            var user = repo.All<ApplicationUser>().Where(u => u.Id == userId).FirstOrDefault();
            return user!.UserName;
        }

        public string GetCategoryName (string cateroryId)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Id == cateroryId).Select(c => c.Name).FirstOrDefault();
        }

        public CategoryTransactions GetCategory (string cateroryId)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Id == cateroryId).FirstOrDefault();
        }

        public Client GetClient (string clientId)
        {
            return repo.AllReadonly<Client>().Where(c => c.Id == clientId).FirstOrDefault();
        }

        public CategoryTransactions GetCategoryByName (string cateroryName)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Name == cateroryName).FirstOrDefault();
        }

        public int GetCountTransactions ()
        {
            return repo.AllReadonly<Transaction>().Where(t => t.IsDeleted == false).Count();
        }

        public Transaction GetTransactionById (string transactionId)
        {
            return repo.All<Transaction>()
                .Where(t => t.Id == transactionId).FirstOrDefault();
        }

        public ICollection<CategoryViewModel> GetCategories ()
        {
            colors.AddRange(colorsArray);
            icons.AddRange(iconsArray);

            List<CategoryViewModel> model = new List<CategoryViewModel>();

            var categories = repo.AllReadonly<CategoryTransactions>().Where(c => c.IsDeleted == false)
                .Select(c => new CategoryViewModel()
                {
                    Name = c.Name!,
                    Id = c.Id,
                    TransactionCount = c.Transactions.Count(),

                }).ToList();

            foreach (var item in categories)
            {
                item.Color = GetColor();
                item.Icon = GetIcon();
                model.Add(item);
            }

            return model;
        }

        private string GetColor ()
        {
            var color = colors.First();
            colors.RemoveAt(0);
            colors.Add(color);
            return color;
        }

        private string GetIcon ()
        {
            var icon = icons.First();
            icons.RemoveAt(0);
            icons.Add(icon);
            return icon;
        }

        public IEnumerable<AllCategoryTransactionsViewModel> GetCategoryTransactions (int page, int itemsPerPage, string categoryId)
        {
            var countTransactions = GetCategoryTransactionsCount(categoryId);

            if (itemsPerPage > countTransactions)
            {
                itemsPerPage = countTransactions;
            }

            return repo.AllReadonly<Transaction>()
                .Where(t => t.IsDeleted == false)
                .Where(t => t.CategoryId == categoryId)
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(t => new AllCategoryTransactionsViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Date = t.Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    TransactionType = t.TransactionType,
                    FileId = t.FileId!,
                    Category = t.Category.Name!,
                    CreatorName = t.CreatorName,
                })
                .ToList();
        }

        public int GetCategoryTransactionsCount (string categoryId)
        {
            int countTransaction = repo.AllReadonly<CategoryTransactions>()
                .Where(c => c.Id == categoryId)
                .Where(c => c.IsDeleted == false)
                .Select(c => c.Transactions.Count())
                .FirstOrDefault();


            return countTransaction;
        }

        public decimal GetTotalAmounthTransactions (string categoryId)
        {
            decimal countTransaction = repo.AllReadonly<CategoryTransactions>()
                .Where(c => c.Id == categoryId)
                .Where(c => c.IsDeleted == false)
                .Select(c => c.Transactions.Sum(t => t.Amount)).FirstOrDefault();
            return countTransaction;
        }
    }
}
