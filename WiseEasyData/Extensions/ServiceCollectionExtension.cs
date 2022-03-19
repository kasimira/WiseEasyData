using Core.Contracts;
using Core.Services;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WiseEasyData.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IApplicatioDbRepository, ApplicatioDbRepository>();
            //services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IIndexAppService, IndexAppService>();
            services.AddTransient<ICommonService, CommonService>();


            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
