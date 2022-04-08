using Core.Contracts;
using Core.Models.Employee;
using Infrastructure.Data;
using Infrastructure.Data.Enums;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using SubmittedFile = Infrastructure.Data.SubmittedFile;

namespace Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApplicatioDbRepository repo;
        private readonly ICommonService commonService;
        private readonly IFileService fileService;

        public EmployeeService (IApplicatioDbRepository _repo, ICommonService _commonService, IFileService _fileService)
        {
            repo = _repo;
            commonService = _commonService;
            fileService = _fileService;
        }

        public async Task<bool> AddEmployeeAsync (AddEmployeeViewModel model, bool created, string rootPath, string userId, string userName)
        {
            var country = commonService.GetCountry(model.Country!);
            var city = commonService.GetCity(model.City!);
            var department = commonService.GetDepartment(model.Department!);

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

            if (department == null)
            {
                department = commonService.CreateDepartment(model.Department!);
                await repo.AddAsync(department);
            }

            string imageId = null!;
            SubmittedFile dbFile = null!;

            if (model.Image != null)
            {
                var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

                dbFile = new SubmittedFile()
                {
                    OwnerId = model.Id,
                    Extension = extension,
                };

                imageId = dbFile.Id;

                Directory.CreateDirectory($"{rootPath}/images/employees/");
                var physicalPath = $"{rootPath}/images/employees/{dbFile.Id}.{extension}";

                using (FileStream fs = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fs);
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
                CityId = city.Id,
                DateOfBirth = model.DateOfBirth.Date,
                DepartmentId = department.Id,
                HourlySalary = model.HourlySalary,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Position = model.Position,
                PostalCode = model.PostalCode,
                Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender!),
                Grade = (Grade)Enum.Parse(typeof(Grade), model.Grade!),
                Status = (Status)Enum.Parse(typeof(Status), model.Status!),
                HireDate = model.HireDate.Date,
                ImageId = imageId,
                CreatorId = userId,
                CreatorName = userName,
            };

            if (employee == null)
            {
                throw new Exception("Employee is null.");
            }

            if (dbFile != null)
            {
                await repo.AddAsync(dbFile);
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

        public IEnumerable<AllEmployeesViewModel> GetEmployees (int page, int itemsPerPage)
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
                    PhoneNumber = e.PhoneNumber!,
                    Position = e.Position!,
                    City = e.City!.Name!,
                    HireDate = e.HireDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                    HourlySalary = e.HourlySalary,
                    Status = e.Status.ToString(),
                })
                .ToList();
        }

        public EmployeeProfileViewModel GetEmployeeProfil (string id)
        {
            var imageId = repo.AllReadonly<Employee>().Where(e => e.Id == id).Select(e => e.ImageId).FirstOrDefault();
            var image = "";

            if (imageId != null)
            {
                var imageInfo = fileService.GetFileById(imageId);
                image = $"{imageId}.{imageInfo!.Extension}";
            }
            else
            {
                image = "contractor-1623889_960_720.jpg";
            }

            var employee = repo.AllReadonly<Employee>().Where(e => e.Id == id)
                .Include(e => e.City)
                .Include(c => c.City!.Country)
                .Include(e => e.Department)
                .Select(e => new EmployeeProfileViewModel()
                {
                    Id = id,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    Nationality = e.Nationality,
                    Address = e.Address,
                    City = e.City!.Name,
                    Country = e.City!.Country!.Name,
                    Department = e.Department!.Name,
                    DateOfBirth = e.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                    Image = image,
                    Gender = e.Gender,
                    Status = e.Status,
                    Grade = e.Grade,
                    HireDate = e.HireDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = e.ReleaseDate,
                    HourlySalary = e.HourlySalary,
                    Position = e.Position,
                    PostalCode = e.PostalCode,
                    DataToAdded = e.DateToAdd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    CreatorName = e.CreatorName,
                })
                .FirstOrDefault();

            return employee;
        }

        public async Task EditEmployeeAsync (EditEmployeeViewModel model, string employeeId, string rootPath)
        {
            var employee = GetEmployee(employeeId);
            var country = commonService.GetCountry(model.Country!);
            var city = commonService.GetCity(model.City!);
            var department = commonService.GetDepartment(model.Department!);

            if (country == null)
            {
                country = commonService.CreateCountry(model.Country!);
            }

            if (city == null)
            {
                city = commonService.CreateCity(model.City!, country);
            }

            if (department == null)
            {
                department = commonService.CreateDepartment(model.Department!);
            }

            var imageId = "";

            if (model.Image != null)
            {
                if (employee.ImageId != null)
                {
                    var OldImage = fileService.GetFileById(employee.ImageId);

                    OldImage!.IsDeleted = true;

                    var fullPath = $"{rootPath}/images/employees/{OldImage.Id}.{OldImage.Extension}";

                    await fileService.DeleteFile(fullPath, OldImage);
                }

                var extension = Path.GetExtension(model.Image.FileName).TrimStart('.');

                var dbImage = new SubmittedFile
                {
                    OwnerId = employeeId,
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
            employee.Gender = (Gender)Enum.Parse(typeof(Gender), model.Gender!);
            employee.Grade = (Grade)Enum.Parse(typeof(Grade), model.Grade!);
            employee.Status = (Status)Enum.Parse(typeof(Status), model.Status!);
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

        public Employee GetEmployee (string employeeId)
        {
            var employee = repo.All<Employee>().Where(e => e.Id == employeeId).FirstOrDefault();
            return employee;
        }

        public async Task DeleteAsync (string employeeId)
        {
            var employee = GetEmployee(employeeId);

            if (employee.ImageId != null)
            {
                fileService.ChangeFileIsDeletedTrue(employee.ImageId);
            }

            employee.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public int GetCount ()
        {
            return repo.All<Employee>().Where(e => e.IsDeleted == false).Count();
        }

        public EditEmployeeViewModel GetEmployeeInfo<T> (string employeeId)
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
                Country = city.Country!.Name,
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

        public async Task ChangeStatusAsync (string employeeId)
        {
            var employee = GetEmployee(employeeId);
            Status status = employee.Status;


            if (status == Status.Active)
            {
                status = (Status)Enum.Parse(typeof(Status), "Inactive");
            }
            else
            {
                status = (Status)Enum.Parse(typeof(Status), "Active");
            }

            employee.Status = status;
            await repo.SaveChangesAsync();
        }

        public DeleteEmployeeViewModel GetEmployeeForDelete (string id)
        {
            var employee = GetEmployee(id);

            var employeeForDelete = new DeleteEmployeeViewModel
            {
                Id = id,
                Name = employee.FirstName + " " + employee.LastName,
                HireDate = employee.HireDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            };

            return employeeForDelete;
        }
    }
}
