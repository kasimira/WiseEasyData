using Core.Contracts;
using Core.Models.Transactions;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;


namespace Core.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly IApplicatioDbRepository repo;
        private readonly ICommonService commonService;

        public TransactionService (IApplicatioDbRepository _repo, ICommonService _commonService)
        {
            repo = _repo;
            commonService = _commonService;
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
                CreatorName = id,
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
                var currentfile = repo.AllReadonly<SubmittedFile>().Where(i => i.Id == fileId).FirstOrDefault();

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

            return transaction;
        }

        public string GetCategoryName (string cateroryId)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Id == cateroryId).Select(c => c.Name).FirstOrDefault();
        }

        public CategoryTransactions GetCategory (string cateroryId)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Id == cateroryId).FirstOrDefault();
        }

        public CategoryTransactions GetCategoryByName (string cateroryName)
        {
            return repo.AllReadonly<CategoryTransactions>().Where(c => c.Name == cateroryName).FirstOrDefault();
        }

        public int GetCountTransactions ()
        {
            return repo.AllReadonly<Transaction>().Where(t => t.IsDeleted == false).Count();
        }

        public async Task DeleteTransactionAsync (string id)
        {
            var transaction = repo.All<Transaction>().Where(e => e.Id == id).FirstOrDefault();

            if (transaction!.FileId != null)
            {
                commonService.DeleteFile(transaction.FileId);
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

            try
            {
                await repo.AddAsync(category);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public EditTransactionViewModel GetTransactionForEdit (string transactionId)
        {
            var transaction = repo.All<Transaction>()
                .Where(t => t.Id == transactionId).FirstOrDefault();
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

        public async Task<bool> EditTransactionAsync (EditTransactionViewModel model, bool edited, string rootPath, object id, string userId)
        {
            SubmittedFile dbFile = null!;

            if (model.File != null)
            {
                dbFile = await CreateFile(model, rootPath, userId);             
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

            var transaction = new Transaction()
            {
                Name = model.Name,
                CategoryId = category.Id,
                Date = model.Date,
                Amount = model.Amount,
                Currency = (Currency)Enum.Parse(typeof(Currency), model.Currency),
                TransactionType = (TransactionType)Enum.Parse(typeof(TransactionType), model.TransactionType),
                FileId = dbFile.Id,
                Description = model.Description,
                CreatorName = userId,
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

            try
            {
                await repo.AddAsync(transaction);
                await repo.SaveChangesAsync();
                edited = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (edited);
        }

        private async Task<SubmittedFile> CreateFile (EditTransactionViewModel model, string rootPath, string userId)
        {
            var extension = Path.GetExtension(model.File!.FileName).TrimStart('.');

            var dbFile = new SubmittedFile()
            {
                OwnerId = userId,
                Extension = extension,
                IsImage = false,
                TransactionName = model.Name,
            };
           
            Directory.CreateDirectory($"{rootPath}/files/transaction/");
            var physicalPath = $"{rootPath}/files/transaction/{dbFile.Id}.{extension}";

            using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
            {
                 await model.File.CopyToAsync(fs);
            }

            return dbFile;
        }
    }
}
