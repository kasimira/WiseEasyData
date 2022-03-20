using Core.Contracts;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using System.Globalization;

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

        public async Task<bool> AddEmployeeAsync(AddEmployeeViewModel model, bool created, string rootPath)
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

            string imageId = null;
 
            if (model.Image != null)
            {
                var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

                var dbImage = new Image()
                {
                    EmployeeId = model.Id,
                    Extension = extension,
                };
                imageId = dbImage.Id;

                Directory.CreateDirectory($"{rootPath}/images/employees/");
                var physicalPath = $"{rootPath}/images/employees/{dbImage.Id}.{extension}";

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fs);

                }

                await repo.AddAsync(dbImage);
                await repo.SaveChangesAsync();
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
                ImageId = imageId,
            };

            if (employee == null)
            {
                throw new Exception("Employee is null.");
            }

            city.Employees.Add(employee);

            if (!country.Cities.Contains(city))
            {
                country.Cities.Add(city);
            }

            department.Employees.Add(employee);

            try
            {
                await repo.AddAsync(employee);
                await repo.SaveChangesAsync();
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

            if (itemsPerPage > countEmployee)
            {
                itemsPerPage = countEmployee;
            }

            return repo.AllReadonly<Employee>()
                .Where(e => e.IsDeleted == false)
                .OrderBy(e => e.FirstName)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
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

        public async Task EditEmployeeAsync(EditEmployeeViewModel model, string employeeId, string rootPath)
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

            var imageId = "";


            if (model.Image != null)
            {

                var imageForDeleteId = repo.All<Employee>().Where(i => i.Id == employeeId).Select(e => e.ImageId).FirstOrDefault();

                if (imageForDeleteId != null)
                {
                    var imageForDelete = repo.All<Image>().Where(i => i.Id == imageForDeleteId).FirstOrDefault();
                    imageForDelete.IsDeleted = true;
                }

                var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

                var dbImage = new Image
                {

                    EmployeeId = employeeId,
                    Extension = extension,
                };

                await repo.AddAsync(dbImage);

                imageId = dbImage.Id;

                Directory.CreateDirectory($"{rootPath}/images/employees/");
                var physicalPath = $"{rootPath}/images/employees/{dbImage.Id}.{extension}";

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fs);
                }


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
            employee.ImageId = imageId;

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

        public async Task DeleteAsync(string employeeId)
        {
            var employee = GetEmployee(employeeId);

            employee.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public int GetCount()
        {
            return repo.All<Employee>().Where(e => e.IsDeleted == false).Count();

        }

        public EditEmployeeViewModel GetEmployeeInfo<T>(string employeeId)
        {
            var e = GetEmployee(employeeId);
            var city = commonService.GetCityById(e.CityId);
            var department = commonService.GetDepartmentById(e.DepartmentId);           

            var employee = new EditEmployeeViewModel
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

            };
            return employee;
        }    
    }
}
