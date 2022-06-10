using Core.Contracts;
using Core.Models.Client;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


namespace Core.Services
{
    public class ClientService : IClientService
    {
        private readonly IApplicatioDbRepository repo;
        private readonly ICommonService commonService;

        public ClientService (IApplicatioDbRepository _repo, ICommonService _commonService)
        {
            repo = _repo;
            commonService = _commonService;
        }

        public async Task<bool> AddClientAsync (AddClientViewModel model, bool created)
        {
            var country = commonService.GetCountry(model.Country!);
            var city = commonService.GetCity(model.City!);

            if (country == null)
            {
                country = commonService.CreateCountry(model.Country!);
                await repo.AddAsync(country);
            }

            if (city == null)
            {
                city = commonService.CreateCity(model.City!, country);
                await repo.AddAsync(city);
            }

            var client = new Client()
            {
                Id = model.Id,
                Name = model.Name,
                VatNumber = model.VatNumber,
                CeoName = model.CeoName,
                Address = model.Address,
                CityId = city.Id,
                PhoneNumber = model.PhoneNumber,
                MobilePhone = model.MobilePhone,
                Email = model.Email,
                Email2 = model.Email2,
                Website = model.Website,
                PostalCode = model.PostalCode,
                Status = (Status)Enum.Parse(typeof(Status), model.Status!),
                HireDate = model.HireDate.Date,
                ReleaseDate = model.ReleaseDate,
                LogoPath = model.LogoPath,
                ConstructionsSites = new List<ConstructionSite>()
            };

            if (client == null)
            {
                throw new Exception("Client is null.");
            }

            if (!country.Cities.Contains(city))
            {
                country.Cities.Add(city);
            }

            try
            {
                await repo.AddAsync(client);
                await repo.SaveChangesAsync();
                created = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (created);
        }

        public IEnumerable<AllClientsViewModel> GetClients (int page, int itemsPerPage)
        {
            var countClient = GetCount();

            if (itemsPerPage > countClient)
            {
                itemsPerPage = countClient;
            }

            return repo.AllReadonly<Client>()
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Name)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(c => new AllClientsViewModel()
                {
                    Id = c.Id,
                    Name = c.Name!,
                    PhoneNumber = c.PhoneNumber!,
                    Email = c.Email!,
                    City = c.City!.Name!,
                    HireDate = c.HireDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    Status = c.Status.ToString(),
                })
                .ToList();
        }

        public ClientProfileViewModel GetClientProfil (string id)
        {
            var client = repo.AllReadonly<Client>().Where(e => e.Id == id)
                .Include(c => c.City)
                .Include(ci => ci.City!.Country)
                .Select(c => new ClientProfileViewModel()
                {
                    Id = id,
                    Name = c.Name,
                    VatNumber = c.VatNumber,
                    CeoName = c.CeoName,
                    Address = c.Address,
                    City = c.City!.Name,
                    Country = c.City!.Country!.Name,
                    Email = c.Email,
                    Email2 = c.Email2,
                    PhoneNumber = c.PhoneNumber,
                    Website = c.Website,
                    MobilePhone = c.MobilePhone,
                    Status = c.Status,
                    HireDate = c.HireDate,
                    ReleaseDate = c.ReleaseDate,
                    PostalCode = c.PostalCode,                  
                })
                .FirstOrDefault();

            return client;
        }

        public async Task EditClientAsync (EditClientViewModel model, string clientId)
        {
            var client = GetClient(clientId);
            var country = commonService.GetCountry(model.Country!);
            var city = commonService.GetCity(model.City!);

            if (country == null)
            {
                country = commonService.CreateCountry(model.Country!);
            }

            if (city == null)
            {
                city = commonService.CreateCity(model.City!, country);
            }

            client.Name = model.Name;
            client.VatNumber = model.VatNumber;
            client.CeoName = model.CeoName;
            client.Address = model.Address;
            client.City = city;
            client.Website = model.Website;
            client.PhoneNumber = model.PhoneNumber;
            client.MobilePhone = model.MobilePhone;
            client.Email = model.Email;
            client.Email2 = model.Email2;
            client.PostalCode = model.PostalCode;
            client.ReleaseDate = model.ReleaseDate;
            client.Status = (Status)Enum.Parse(typeof(Status), model.Status!);
            client.HireDate = model.HireDate.Date;

            if (!country.Cities.Contains(city))
            {
                country.Cities.Add(city);
            }

            await repo.SaveChangesAsync();
        }

        public Client GetClient (string clientId)
        {
            var client = repo.All<Client>().Where(e => e.Id == clientId).FirstOrDefault();
            return client;
        }

        public async Task DeleteAsync (string clientId)
        {
            var client = GetClient(clientId);

            client.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public int GetCount ()
        {
            return repo.All<Client>().Where(e => e.IsDeleted == false).Count();
        }

        public EditClientViewModel GetClientInfo<T> (string clientId)
        {
            var currentClient = GetClient(clientId);
            var city = commonService.GetCityById(currentClient.CityId);

            var client = new EditClientViewModel
            {
                Id = currentClient.Id,
                Name = currentClient.Name,
                CeoName = currentClient.CeoName,
                VatNumber = currentClient.VatNumber,
                Address = currentClient.Address,
                City = city.Name,
                Country = city.Country!.Name,
                PhoneNumber = currentClient.PhoneNumber,
                MobilePhone = currentClient.MobilePhone,
                Email = currentClient.Email,
                Email2 = currentClient.Email2,
                Website = currentClient.Website,
                Status = currentClient.Status.GetTypeCode().ToString(),
                HireDate = currentClient.HireDate.Date,
                ReleaseDate = currentClient.ReleaseDate,
                PostalCode = currentClient.PostalCode,
            };
            return client;
        }

        public async Task ChangeStatusAsync (string clientId)
        {
            var client = GetClient(clientId);
            Status status = client.Status;


            if (status == Status.Active)
            {
                status = (Status)Enum.Parse(typeof(Status), "Inactive");
            }
            else
            {
                status = (Status)Enum.Parse(typeof(Status), "Active");
            }

            client.Status = status;
            await repo.SaveChangesAsync();
        }

        public DeleteClientViewModel GetClientForDelete (string id)
        {
            var client = GetClient(id);

            var clientForDelete = new DeleteClientViewModel
            {
                Id = id,
                Name = client.Name!,
                HireDate = client.HireDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            return clientForDelete;
        }

        public int GetCountInvoices (string clientId)
        {
            int number = repo.AllReadonly<Transaction>().Where(c => c.ClientId == clientId).Count();
            
            return number;
        }

        public IEnumerable<AllInvoicesViewModel> GetInvoices (int page, int itemsPerPage, string clientId, int invoicesCount)
        {
            if (itemsPerPage > invoicesCount)
            {
                itemsPerPage = invoicesCount;
            }

            return repo.All<Transaction>()
                .Where(t => t.ClientId == clientId)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .Select(t => new AllInvoicesViewModel()
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

        public string GetClientName (string clientId)
        {
            return repo.AllReadonly<Client>().Where(c => c.Id == clientId).Select(c => c.Name).FirstOrDefault();
        }

        public decimal GetTotalAmountInvoices (string clientId)
        {
            decimal number = repo.AllReadonly<Transaction>()
                .Where(c => c.ClientId == clientId)
                .Select(c => c.Amount)
                .Sum();

            return number;
        }
    }
}
