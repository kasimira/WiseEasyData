using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using System.Globalization;
using System.Diagnostics;

namespace Core.Services
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IApplicatioDbRepository repo;
        private readonly ICommonService commonService;

        public EmployeeService(IApplicatioDbRepository _repo, ICommonService _commonService)
        {
            repo = _repo;
            commonService = _commonService;
        }

        public async Task<bool> AddEmployeeAsync(AddEmployeeViewModel model, bool created)
        {           
            var country = commonService.GetCountry(model.Country);
            var city = commonService.GetCity(model.City);
            var department = commonService.GetDepartment(model.Department);

            if (country == null)
            {
                country = commonService.CreateCountry(model.Country);
            }

            if (city == null)
            {
                city = commonService.CreateCity(model.City, country);
            }

            if (department == null)
            {
                department = commonService.CreateDepartment(model.Department);
            }

            if (model.Image != null)
            {
                if (!model.Image.Name.EndsWith(".png") || !model.Image.Name.EndsWith(".jpg"))
                {

                }
            }

            var employee = new Employee()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Nationality = model.Nationality,
                Address = model.Address,
                City = city,
                DateOfBirth = model.DateOfBirth.Date,
                Department = department,
                HourlySalary = model.HourlySalary,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Position = model.Position,
                PostalCode = model.PostalCode,
                Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender),
                Grade = (Grade)Enum.Parse(typeof(Grade), model.Grade),
                Status = (Status)Enum.Parse(typeof(Status), model.Status),
                HireDate = model.HireDate.Date,
                
            };

            if (employee == null)
            {
                throw new ArgumentException();
            }

            city.Employees.Add(employee);

            if (!country.Cities.Contains(city))
            {
                country.Cities.Add(city);
            }

            department.Employees.Add(employee);


            try
            {
               await this.repo.AddAsync(employee);
               await this.repo.SaveChangesAsync();
               created = true;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }

            return (created);

        }

        public IEnumerable<AllEmployeesViewModel> GetEmployees(int page, int itemsPerPage)
        {

            var countEmployee = GetCount();

            if(itemsPerPage > countEmployee)
            {
                itemsPerPage = countEmployee;
            }

            return repo.AllReadonly<Employee>()
                .Where(e => e.IsDeleted == false)
                .OrderBy(e => e.FirstName)
                .Skip((page - 1)  * itemsPerPage).Take(itemsPerPage) 
                .Select(e => new AllEmployeesViewModel()
                {
                    Id = e.Id,
                    FullName = e.FirstName + " " + e.MiddleName + " " + e.LastName,
                    PhoneNumber = e.PhoneNumber,
                    Position = e.Position,
                    City = e.City.Name,
                    HireDate = e.HireDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    HourlySalary = e.HourlySalary,
                    Status = e.Status.ToString(),           
                })
                .ToList();
        }

        public void AddEmployeeToConstructionSite(string employeeId)
        {
            // var daysThisMonthThatAreNotSundays =
            // Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Where(
            //     d => new DateTime(DateTime.Now.Year, DateTime.Now.Month, d).DayOfWeek != DayOfWeek.Sunday).Count();

            var daysThisMonthThatAreSatadey =
            Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Where(
                d => new DateTime(DateTime.Now.Year, DateTime.Now.Month, d).DayOfWeek == DayOfWeek.Saturday).Count();
            var sites = repo.All<ConstructionSite>();

            if (sites.Count() > 0)
            {
                foreach (var item in sites)
                {
                    if (DateTime.Now.Month == item.StartingDate.Month)
                    {
                        var daysThisMonthThatAreNotSundays =
                         Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).Where(
                          d => new DateTime(DateTime.Now.Year, DateTime.Now.Month, d).DayOfWeek != DayOfWeek.Sunday).Count();
                    }

                }

            }


            // define non working days of week
            var nonWorkingDaysOfWeek = new List<DayOfWeek>
            {
              DayOfWeek.Sunday // hard-coded for example
            };

            // define specific non-working dates
            var holidays = new List<DateTime>
            {
              new DateTime(2010, 12, 25) // hard-coded for example
            };

            // tally the working days
            var currentYear = 2010; // hard-coded for example
            var currentMonth = 12; // hard-coded for example
            var daysInCurrentMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var numberOfWorkingDays = 0;
            for (var day = 1; day <= daysInCurrentMonth; day++)
            {
                var date = new DateTime(currentYear, currentMonth, day);
                if (!nonWorkingDaysOfWeek.Contains(date.DayOfWeek) && !holidays.Contains(date))
                {
                    numberOfWorkingDays++;
                }
            }
        }

        public async Task EditEmployeeAsync(AddEmployeeViewModel model, string employeeId)
        {
            var employee = GetEmployee(employeeId);
            var country = commonService.GetCountry(model.Country);
            var city = commonService.GetCity(model.City);
            var department = commonService.GetDepartment(model.Department);

            if (country == null)
            {
                country = commonService.CreateCountry(model.Country);
            }

            if (city == null)
            {
                city = commonService.CreateCity(model.City, country);
            }

            if (department == null)
            {
                department = commonService.CreateDepartment(model.Department);
            }

            if (model.Image != null)
            {
                //if (!model.Image.Name.EndsWith(".png") || !model.Image.Name.EndsWith(".jpg"))
                //{
                //
                //}
            }

            employee.FirstName = model.FirstName;
            employee.MiddleName = model.MiddleName;
            employee.LastName = model.LastName;
            employee.Nationality = model.Nationality;
            employee.Address = model.Address;
            employee.City = city;
            employee.DateOfBirth = model.DateOfBirth;
            employee.Department = department;
            employee.HourlySalary = model.HourlySalary;
            employee.PhoneNumber = model.PhoneNumber;
            employee.Email = model.Email;
            employee.Position = model.Position;
            employee.PostalCode = model.PostalCode;
            employee.Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender);
            employee.Grade = (Grade)Enum.Parse(typeof(Grade), model.Grade);
            employee.Status = (Status)Enum.Parse(typeof(Status), model.Status);
            employee.HireDate = model.HireDate.Date;
            //employee.Image = 

            city.Employees.Add(employee);

            if (!country.Cities.Contains(city))
            {
                country.Cities.Add(city);
            }

            department.Employees.Add(employee);

            await repo.SaveChangesAsync();

        }

        public Employee GetEmployee(string employeeId)
        {
            var employee = repo.All<Employee>().Where(e => e.Id == employeeId).FirstOrDefault();
            return employee;
        }

        public EditEmployeeViewModel GetEmployeeForChange(string employeeId)
        {
            var e = GetEmployee(employeeId);
            var city = commonService.GetCityById(e.CityId);
            var department = commonService.GetDepartmentById(e.DepartmentId);
            var image = repo.All<Image>().Where(i => i.Id == e.ImageId).FirstOrDefault();

            var employee = new EditEmployeeViewModel()
            {
                Id = e.Id,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                LastName = e.LastName,
                Nationality = e.Nationality,
                Address = e.Address,
                City = city.Name,
                Country = city.Country.Name,
                DateOfBirth = e.DateOfBirth,
                Department = department.Name,
                HourlySalary = e.HourlySalary,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email,
                Position = e.Position,
                PostalCode = e.PostalCode,
                Gender = e.Gender.GetTypeCode().ToString(),
                Grade = e.Grade.GetTypeCode().ToString(),
                Status = e.Status.GetTypeCode().ToString(),
                HireDate = e.HireDate.Date,
                Image = "/images/employees/" + e.ImageId + "." + image.Extension,
            };
            return employee;
        }

        public async Task DeleteAsync(string employeeId)
        {
            var employee = GetEmployee(employeeId);

            employee.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.repo.All<Employee>().Where(e => e.IsDeleted == false).Count();

        }
    }
}
