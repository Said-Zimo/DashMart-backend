

using DashMart.Domain.People.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.ToTable("Customers");

            builder.Property(x => x.Balance)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerId);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

        }


    }
}
