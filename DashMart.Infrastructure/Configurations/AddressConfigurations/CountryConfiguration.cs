

using DashMart.Domain.Addresses.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations.AddressConfigurations
{
    public class CountryConfiguration : EntityConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            base.Configure(builder);

            builder.ToTable("Countries");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

        }

    }
}
