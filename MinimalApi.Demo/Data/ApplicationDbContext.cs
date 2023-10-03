using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Demo.Models;
using static Azure.Core.HttpHeader;

namespace MinimalApi.Demo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    Id = 1,
                    Name = "10OFF",
                    Price = 10,
                    IsActive = true,
                },
                new Product()
                {
                    Id = 2,
                    Name = "20OFF",
                    Price = 20,
                    IsActive = true,
                });
        }
    }
}
