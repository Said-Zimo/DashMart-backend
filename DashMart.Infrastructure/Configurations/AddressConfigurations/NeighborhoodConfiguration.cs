

using DashMart.Domain.Addresses.Neighborhoods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations.AddressConfigurations
{
    public class NeighborhoodConfiguration : EntityConfiguration<Neighborhood>
    {

        public override void Configure(EntityTypeBuilder<Neighborhood> builder)
        {
            base.Configure(builder);

            builder.ToTable("Neighborhoods");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();


            builder.HasOne(x => x.District)
                .WithMany(x=> x.Neighborhoods)
                .HasForeignKey(x=> x.DistrictId)
                .IsRequired();

        }

    }
}
