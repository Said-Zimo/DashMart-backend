

using DashMart.Domain.People.Couriers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class CourierConfiguration : IEntityTypeConfiguration<Courier>
    {

         public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.ToTable("Couriers");

            builder.Property(x => x.IsReadyToWork)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasMany(x => x.DriverLicenses)
                .WithOne(x => x.Courier)
                .HasForeignKey(x => x.CourierId);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

        }
    }
}
