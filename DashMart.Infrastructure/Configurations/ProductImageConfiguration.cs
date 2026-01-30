

using DashMart.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class ProductImageConfiguration :EntityConfiguration<ProductImageGallery>
    {

        public override void Configure(EntityTypeBuilder<ProductImageGallery> builder)
        {
            base.Configure(builder);


            builder.ToTable("ProductImageGallery");


            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.ImagePath)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.IsMain)
                .HasColumnType("BIT")
                .IsRequired();


        }


    }
}
