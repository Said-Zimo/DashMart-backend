

using DashMart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class ProductConfiguration : EntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {

            base.Configure(builder);

            builder.ToTable("Products");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.OwnsOne(x => x.Weight, c =>
            {
                c.Property(x => x.Value).HasColumnName("Weight").IsRequired();
            });

            builder.OwnsOne(x => x.Price, c =>
            {
                c.Property(x => x.Value).HasColumnName("Price").HasColumnType("DECIMAL(18,2)").IsRequired();
            });

            builder.Property(x => x.HowToUseNote)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(250);

            builder.OwnsOne(x => x.SKU, c =>
            {
                c.HasIndex(x => x.Value).IsUnique();
                c.Property(x => x.Value).HasColumnName("SKU").HasMaxLength(20).IsRequired();
            });


            builder.Property(x => x.StockQuantity)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnType("BIT")
                .IsRequired();

            builder.HasMany(x => x.ProductImages)
                .WithOne(x => x.Product)
                .HasForeignKey(x=> x.ProductId);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();


        }
    }
}
