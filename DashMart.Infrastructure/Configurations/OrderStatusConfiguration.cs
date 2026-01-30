

using DashMart.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class OrderStatusConfiguration : EntityConfiguration<OrderStatus>
    {
        public override void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable("OrderStatus");

            builder.Property(x => x.Name)
                .HasMaxLength(20)
                .IsRequired();

        }
    }
}
