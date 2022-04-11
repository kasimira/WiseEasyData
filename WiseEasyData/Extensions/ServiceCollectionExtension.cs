using Core.Contracts;
using Core.Services;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using WiseEasyData.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddTransient<IApplicatioDbRepository, ApplicatioDbRepository>();
            //services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IIndexAppService, IndexAppService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISalaryService, SalaryService>();
            services.AddTransient<IClientService, ClientService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts (this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
