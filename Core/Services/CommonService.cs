using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
namespace Core.Services
{
    public class CommonService : ICommonService
    {
        private readonly IApplicatioDbRepository repo;

        public CommonService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public Country GetCountry(string countryName)
        {
            Country? country = repo.All<Country>().FirstOrDefault(c => c.Name == countryName);

            return country;
        }
        public City GetCity(string cityName)
        {
            City? city = repo.All<City>().FirstOrDefault(c => c.Name == cityName);

            return city;
        }
        public Department GetDepartment(string departmantName)
        {
            Department? department = repo.All<Department>().FirstOrDefault(d => d.Name == departmantName);

            return department;
        }

        public City CreateCity(string cityName, Country country)
        {
            var city = new City()
            {
                Name = cityName,
                Country = country,
                CountryId = country.Id,
                Employees = new List<Employee>()
            };
            return city;
        }

        public Country CreateCountry(string countryName)
        {
            var country = new Country()
            {
                Name = countryName,
                Cities = new List<City>()
            };
            return country;
        }

        public Department CreateDepartment(string departmentName)
        {
            var department = new Department()
            {
                Name = departmentName
            };
            return department;
        }

        public City GetCityById(string cityId)
        {
            City? city = repo.All<City>().FirstOrDefault(c => c.Id == cityId);

            city.Country = repo.All<Country>().FirstOrDefault(c => c.Id == city.CountryId);

            return city;
        }

        public Department GetDepartmentById(string departmentId)
        {
            Department? department = repo.All<Department>().FirstOrDefault(d => d.Id == departmentId);

            return department;
        }
    }
}
