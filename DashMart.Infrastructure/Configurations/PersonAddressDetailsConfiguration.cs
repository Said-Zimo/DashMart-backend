

using DashMart.Domain.People.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class PersonAddressDetailsConfiguration : EntityConfiguration<PersonAddressDetails>
    {
        public override void Configure(EntityTypeBuilder<PersonAddressDetails> builder)
        {
            base.Configure(builder);

            builder.ToTable("PersonAddressesDetails");

            builder.Property(x => x.Title)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x=> x.AddressType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.BuildingNo)
                .HasMaxLength(15);


            builder.Property(x => x.HouseNumber)
                .HasMaxLength(15)
                .IsRequired();

            builder.HasOne(x => x.Street)
                .WithMany()
                .HasForeignKey(x => x.StreetId)
                .IsRequired();

        }
    }
}
