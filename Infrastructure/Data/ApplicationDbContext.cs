using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace WiseEasyData.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ConstructionSite> ConstructionSites { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<CategoryTransactions> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SubmittedFile> SubmittedFiles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<Salary>()
                   .HasOne(e => e.Employee)
                   .WithMany(s => s.Salaries)
                   .HasForeignKey(e => e.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(s => s.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Employee>()
                .HasOne(d => d.Department)
                .WithMany(e => e.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

