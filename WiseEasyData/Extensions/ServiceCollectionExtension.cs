﻿using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using WiseEasyData.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicatioDbRepository, ApplicatioDbRepository>();

            return services;
        }

       //public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration config)
       //{
       //    var connectionString = config.GetConnectionString("DefaultConnection");
       //    services.AddDbContext<ApplicationDbContext>(options =>
       //        options.UseSqlServer(connectionString));
       //    services.AddDatabaseDeveloperPageExceptionFilter();
       //
       //    return services;
       //}
    }
}