

using DashMart.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .IsRequired();

            builder.HasOne(x => x.Product)
               .WithMany()
               .HasForeignKey(x => x.ProductId)
               .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Ignore(x=> x.TotalLinePrice);

        }
    }
}
