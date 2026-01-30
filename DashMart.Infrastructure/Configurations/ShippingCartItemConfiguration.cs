

using DashMart.Domain.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class ShippingCartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("ShippingCartItems");

            builder.HasKey(x => new { x.CartId, x.ProductId });

            builder.HasOne(x => x.Cart)
                .WithMany(x=> x.CartItems)
                .HasForeignKey(x=> x.CartId)
                .IsRequired();

            builder.HasOne(x=> x.Product)
                .WithMany()
                .HasForeignKey(x=> x.ProductId)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("DECIMAL(18,2)")
                .IsRequired();

            builder.Ignore(x => x.TotalLinePrice);


        }
    }
}
