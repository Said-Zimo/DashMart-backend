

using DashMart.Domain.Abstraction;
using DashMart.Domain.People.Couriers;
using DashMart.Domain.People.Couriers.DriverLicenses;
using DashMart.Domain.People.xCustomer;
using DashMart.Domain.People.Users;
using DashMart.Domain.Addresses.Cities;
using DashMart.Domain.Addresses.Countries;
using DashMart.Domain.Addresses.Districts;
using DashMart.Domain.Addresses.Neighborhoods;
using DashMart.Domain.Addresses.Streets;
using DashMart.Domain.Carts;
using DashMart.Domain.Categories;
using DashMart.Domain.Orders;
using DashMart.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace DashMart.Infrastructure.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Cart> Carts { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<DriverLicense> DriverLicenses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach(var item in ChangeTracker.Entries<Entity>())
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.CratedAt = DateTime.Now;
                        item.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        item.Entity.UpdatedAt = DateTime.Now;
                        break;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
