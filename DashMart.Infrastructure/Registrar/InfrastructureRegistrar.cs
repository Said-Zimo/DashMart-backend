

using DashMart.Application.Auth;
using DashMart.Application.PasswordHashing;
using DashMart.Application.Carts.Query.Interface;
using DashMart.Application.Categories.Query.Interface;
using DashMart.Application.Couriers.Query.Interface;
using DashMart.Application.Customers.Query.Interface;
using DashMart.Application.Orders.Query.Interface;
using DashMart.Application.Products.Queries.Interface;
using DashMart.Application.Users.Query.Interface;
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
using DashMart.Domain.UnitOfWorks;
using DashMart.Infrastructure.Addresses.Cities;
using DashMart.Infrastructure.Addresses.Countries;
using DashMart.Infrastructure.Addresses.Districts;
using DashMart.Infrastructure.Addresses.Neighborhoods;
using DashMart.Infrastructure.Addresses.Streets;
using DashMart.Infrastructure.Auth;
using DashMart.Infrastructure.Categories;
using DashMart.Infrastructure.Context;
using DashMart.Infrastructure.Couriers;
using DashMart.Infrastructure.Couriers.DriverLicenses;
using DashMart.Infrastructure.Customers;
using DashMart.Infrastructure.Orders;
using DashMart.Infrastructure.PasswordHashing;
using DashMart.Infrastructure.Products;
using DashMart.Infrastructure.ShippingCarts;
using DashMart.Infrastructure.Users;
using DashMart.Infrastructure.xUnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DashMart.Infrastructure.Registrar
{
    public static class InfrastructureRegistrar
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("Default")!;

            if (connectionString == null)
                throw new Exception("Connection string not found");

            services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(connectionString));

            services.AddScoped< IProductRepository,ProductRepository>();
            services.AddScoped<IProductReadRepository,ProductReadRepository>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserReadRepository,UserReadRepository>();

            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<ICourierReadRepository, CourierReadRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();

            services.AddScoped<ICartRepository, ShippingCartRepository>();
            services.AddScoped<ICartReadRepository,  ShippingCartReadRepository>();

            services.AddScoped<IDriverLicenseRepository, DriverLicenseRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
            services.AddScoped<IStreetRepository, StreetRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            var sc = configuration["DashMart_JWT_SECRET_KEY"];

            if (sc == null)
                throw new Exception("Secret key not found");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer
                (op =>
                {
                    op.RequireHttpsMetadata = true;
                    op.SaveToken = true;
                    op.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sc))
                    };
                }
                );



            return services;
        }

    }
}
