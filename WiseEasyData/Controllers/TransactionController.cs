using Core.Contracts;
using Core.Models.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WiseEasyData.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITransactionService transactionService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TransactionController (ILogger<HomeController> logger,
            ITransactionService _transactionService,
            IWebHostEnvironment _webHostEnvironment)
        {
            _logger = logger;
            transactionService = _transactionService;
            webHostEnvironment = _webHostEnvironment;
        }

        public IActionResult All (int id = 1)
        {
            const int ItemsPerPage = 6;
            var viewModel = new TransactionsListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                TransactionsCount = transactionService.GetCountTransactions(),
                Transactions = transactionService.GetTransactions(id, ItemsPerPage),
            };
            return View(viewModel);
        }

        public IActionResult AllCostsByCategory ()
        {
            return View();
        }

        public IActionResult Add ()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add (AddTransactionViewModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool created = false;
            var rootPath = webHostEnvironment.WebRootPath;

            try
            {
                created = await transactionService.AddTransactionAsync(model, created, rootPath, id, userId);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                await transactionService.AddTransactionAsync(model, created, rootPath, id, userId);
            }

            if (!created)
            {
                return Redirect("/Transaction/Error");
            }

            return Redirect("/Transaction/All");
        }

        public IActionResult Get (string transactionId)
        {
            var viewModel = transactionService.GetTransactionInfo(transactionId);

            return View(viewModel);
        }

        public async Task<IActionResult> Delete (string transactionId)
        {
            try
            {
                await transactionService.DeleteTransactionAsync(transactionId);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }

            return Redirect("/Transaction/All");
        }

        public IActionResult AddCategory ()
        {
            return View();
        }
    }
}
