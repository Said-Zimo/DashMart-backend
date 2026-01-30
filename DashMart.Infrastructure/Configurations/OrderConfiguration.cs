

using DashMart.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class OrderConfiguration : EntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.ToTable("Orders");

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId);


            builder.HasOne(x => x.Courier)
                .WithMany()
                .HasForeignKey(x => x.CourierId);
            
            
            builder.Property(x => x.Note)
                .HasMaxLength(50);

            builder.Property(x => x.StreetId)
                .IsRequired();

            builder.HasOne(x=> x.Street)
                .WithMany()
                .HasForeignKey(x => x.StreetId)
                .IsRequired();

            builder.Property(x => x.BuildingNo)
                .HasMaxLength(15);

            builder.Property(x=> x.HouseNo)
                .HasMaxLength(15)
                .IsRequired();

            builder.Ignore(x => x.StatusEnum);

            builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .IsRequired();

            builder.HasMany(x => x.OrderStatusLogs)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

        }


    }
}
