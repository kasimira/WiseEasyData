using Infrastructure.Data.Enums;

namespace Core.Models.Client
{
    public class ClientProfileViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? VatNumber { get; set; }

        public string? Name { get; set; }

        public string? CeoName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? MobilePhone { get; set; }

        public string? Email { get; set; }

        public string? Email2 { get; set; }

        public string? Website { get; set; }

        public DateTime HireDate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string? LogoPath { get; set; }

        public Status Status { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

    }
}
