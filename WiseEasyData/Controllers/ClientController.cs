using Core.Constants;
using Core.Contracts;
using Core.Models.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WiseEasyData.Controllers
{
    public class ClientController : BaseController
    {
        private readonly IClientService clientService;

        public ClientController (IClientService _clientService)
        {
            clientService = _clientService; 
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult All (int id = 1)
        {
            const int ItemsPerPage = 6;

            var viewModel = new ClientsListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                ClientCount = clientService.GetCount(),
                Clients = clientService.GetClients(id, ItemsPerPage),
            };

            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor},{UserConstants.Roles.Guest}")]
        public IActionResult Profile (string clientId)
        {
            var viewModel = clientService.GetClientProfil(clientId);

            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public async Task<IActionResult> ChangeStatus (string clientId)
        {
            await clientService.ChangeStatusAsync(clientId);

            return Redirect("/Client/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor}")]
        public IActionResult Add ()
        {
            return View();
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        [HttpPost]
        public async Task<IActionResult> Add (AddClientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool created = false;

            try
            {
                created = await clientService.AddClientAsync(model, created);
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                await clientService.AddClientAsync(model, created);
            }

            if (!created)
            {
                return Redirect("/Client/Add");
            }

            return Redirect("/Client/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public IActionResult Edit (string clientId)
        {
            var client = clientService.GetClientInfo<EditClientViewModel>(clientId);

            return View(client);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        [HttpPost]
        public async Task<IActionResult> Edit (EditClientViewModel model, string clientId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await clientService.EditClientAsync(model, clientId);

            return Redirect("/Client/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public IActionResult Invoices (string clientId, int id = 1)
        {
            const int ItemsPerPage = 6;
            var invoicesCount = clientService.GetCountInvoices(clientId);

            var viewModel = new InvoicesListViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                InvoicesCount = invoicesCount,
                Invoices = clientService.GetInvoices(id, ItemsPerPage, clientId, invoicesCount),
                ClientName = clientService.GetClientName(clientId),
                TotalAmount = clientService.GetTotalAmountInvoices(clientId),
            };

            return View(viewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public async Task<IActionResult> DeleteClient (string clientId)
        {
            await clientService.DeleteAsync(clientId);

            return Redirect("/Client/All");
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.Editor} ")]
        public IActionResult Delete (string clientId)
        {
            var model = clientService.GetClientForDelete(clientId);
            return View(model);
        }
    }
}
