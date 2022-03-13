using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using System.Globalization;

namespace Core.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IApplicatioDbRepository repo;

        public EmployeeService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public (bool created, string error) AddEmployee(AddEmployeeViewModel model)
        {
            bool created = false;
            string error = null;

            var country = repo.All<Country>().FirstOrDefault(c => c.Name == model.Country);
            var city = repo.All<City>().FirstOrDefault(c => c.Name == model.City);
            var salary = repo.All<Salary>().FirstOrDefault(s => s.HourlySalary == model.Salary);




            if (country == null)
            {
                country = new Country()
                {
                    Name = model.Country,
                    Cities = new List<City>()
                };
            }

            if(city == null)
            {
                city = new City()
                {
                    Name = model.City,
                    Country = country,
                    CountryId = country.CountryId,
                    Employees = new List<Employee>()
                };
            }

            if(salary == null)
            {
                salary = new Salary()
                {
                    HourlySalary = model.Salary,

                };
            }


            if (model.Image != null)
            {
                if(!model.Image.Name.EndsWith(".png") || !model.Image.Name.EndsWith(".jpg"))
                {
                    
                }

            }


            var employee = new Employee()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                EGN = model.EGN,    
                Nationality = model.Nationality,
                Address = model.Address,
                Country = country,
                City = city,
                DateOfBirth = model.DateOfBirth,
                Department = model.Department,
                Salary = salary,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Position = model.Position,
                PostalCode = model.PostalCode,
                NumberOfPersonalId = model.NumberOfPersonalId.ToString(),
                Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender),
                Grade = (Grade)Enum.Parse(typeof(Grade), model.Grade), 
                Status = (Status)Enum.Parse(typeof(Status), model.Status),
                HireDate = model.HireDate

            };
            
            city.Employees.Add(employee);
            country.Cities.Add(city);


            try
            {
                repo.AddAsync(employee);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                error = "Could not save Employee";
            }

            return (created, error);

        }


        public IEnumerable<AllEmployeesViewModel> GetEmployees()
        {
            return repo.All<Employee>()
                .Where(e => e.IsDeleted == false )
                .Select(e => new AllEmployeesViewModel()
                {
                    FullName = e.FirstName + " " + e.MiddleName+ " " + e.LastName,
                    PhoneNumber = e.PhoneNumber,
                    Position = e.Position,
                    City = e.City.Name,
                    HireDate = e.HireDate,
                    Salary = e.Salary.HourlySalary,
                    Status = e.Status.ToString(),

                })
                .ToList();
        }
    }
}
