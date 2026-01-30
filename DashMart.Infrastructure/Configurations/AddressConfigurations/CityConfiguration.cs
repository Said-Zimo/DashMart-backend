

using DashMart.Domain.Addresses.Cities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations.AddressConfigurations
{
    public class CityConfiguration : EntityConfiguration<City>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            base.Configure(builder);

            builder.ToTable("Cities");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.Country)
                .WithMany(x => x.Cities)
                .HasForeignKey(x => x.CountryId)
                .IsRequired();
        }
    }
}
