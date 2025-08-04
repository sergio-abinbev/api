using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.ValueObjects; 

namespace EmployeeManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(builder =>
            {
                builder.HasKey(e => e.Id);
                
                builder.HasIndex(e => e.DocNumber)
                       .IsUnique();
                
                builder.HasIndex(e => e.Email)
                       .IsUnique();
                
                builder.OwnsMany(e => e.Phones, phoneBuilder =>
                {
                    phoneBuilder.ToTable("EmployeePhones");
                    phoneBuilder.HasKey("Id"); 
                    phoneBuilder.Property(p => p.Number).IsRequired();
                    phoneBuilder.Property(p => p.Type).IsRequired().HasMaxLength(50); 
                });
                
                builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                builder.Property(e => e.PasswordHash).IsRequired();
                builder.Property(e => e.DateOfBirth).IsRequired();
                builder.Property(e => e.ManagerName).HasMaxLength(200); 
            });
        }
    }
}