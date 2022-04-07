using Infrastructure.Data;

namespace Core.Contracts
{
    public interface ICommonService
    {
        public City GetCity (string cityName);

        public City CreateCity (string cityName, Country country);

        public Country GetCountry (string countryName);

        public Country CreateCountry (string countryName);

        public Department GetDepartment (string departmentName);

        public Department CreateDepartment (string departmentName);

        public City GetCityById (string cityId);

        public Department GetDepartmentById (string departmentId);

        public SubmittedFile GetFileById (string imageId);
    }
}
