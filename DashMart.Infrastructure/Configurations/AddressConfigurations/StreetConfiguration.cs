using DashMart.Domain.Addresses.Streets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations.AddressConfigurations
{
    public sealed class StreetConfiguration : EntityConfiguration<Street>
    {

        public override void Configure(EntityTypeBuilder<Street> builder)
        {
            base.Configure(builder);


            builder.ToTable("Streets");

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(x => x.Neighborhood)
                .WithMany(x => x.Streets)
                .HasForeignKey(x => x.NeighborhoodID)
                .IsRequired();


        }

    }
}
