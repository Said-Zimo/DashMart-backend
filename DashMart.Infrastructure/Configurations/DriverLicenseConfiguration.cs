

using DashMart.Domain.People.Couriers.DriverLicenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class DriverLicenseConfiguration : EntityConfiguration<DriverLicense>
    {

        public override void Configure(EntityTypeBuilder<DriverLicense> builder)
        {
            base.Configure(builder);

            builder.ToTable("DriverLicenses");

            builder.Property(x => x.CourierId)
                .IsRequired();

            builder.Property(x => x.LicenseNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder.HasIndex(x => x.LicenseNumber)
                .IsUnique();

            builder.Property(x => x.LicenseType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x=> x.StartDate)
                .HasColumnType("DATE")
                .IsRequired();


            builder.Property(x => x.ExpiryDate)
                .HasColumnType("DATE")
                .IsRequired();

            builder.Property(x=> x.IsEnabled)
                .HasColumnType("BIT")
                .IsRequired();


            builder.Property(x=> x.FrontImagePath)
                .HasMaxLength(300)
                .IsRequired();


            builder.Property(x => x.BackImagePath)
                .HasMaxLength(300)
                .IsRequired();

        }

    }
}
