

using DashMart.Domain.Categories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class CategoryConfiguration : EntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);


            builder.Property(x => x.CategoryName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.CategoryName)
                .IsUnique();

            builder.Property(x => x.RowVersion)
                .IsRowVersion();
        }
    }
}
