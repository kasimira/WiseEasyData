using Infrastructure.Data.Enums;
using Microsoft.AspNetCore.Http;
namespace Core.Models.Employee
{
    public class EmployeeProfileViewModel
    {
        public string Id { get; set; }
        public string? Nationality { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Position { get; set; }

        public string? Department { get; set; }

        public string HireDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string DataToAdded { get; set; }

        public string Image { get; set; }

        public int HourlySalary { get; set; }

        public Grade Grade { get; set; }

        public Status Status { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }
    }
}
