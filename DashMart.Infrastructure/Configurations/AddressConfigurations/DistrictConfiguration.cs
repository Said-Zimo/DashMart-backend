

using DashMart.Domain.Addresses.Districts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations.AddressConfigurations
{
    public class DistrictConfiguration : EntityConfiguration<District>
    {
        public override void Configure(EntityTypeBuilder<District> builder)
        {
            base.Configure(builder);

            builder.ToTable("District");

            builder.HasOne(x => x.City)
                .WithMany(x => x.Districts)
                .HasForeignKey(x => x.CityId)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
