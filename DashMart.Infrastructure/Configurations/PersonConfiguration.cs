

using DashMart.Domain.People.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class PersonConfiguration : EntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder.ToTable("Persons");

            builder.Property(x => x.FirstName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
              .HasMaxLength(300)
              .IsRequired();

            builder.OwnsOne(x => x.Phone1, c =>
            {
                c.Property(x => x.Value).HasColumnName("PrimaryPhone").HasMaxLength(11).IsRequired();
                c.HasIndex(x => x.Value).IsUnique();

            });


            builder.OwnsOne(x => x.Phone2, c =>
            {
                c.Property(x => x.Value).HasColumnName("SecondPhone").HasMaxLength(11);
                c.HasIndex(x => x.Value).IsUnique().HasFilter("[SecondPhone] IS NOT NULL");

            });


            builder.OwnsOne(x => x.Email, c =>
            {
                c.Property(x => x.Value).HasColumnName("Email").HasMaxLength(30);
                c.HasIndex(x => x.Value).IsUnique().HasFilter("[Email] IS NOT NULL");
            });

            builder.Property(x => x.IsActive)
                .HasColumnType("BIT")
                .IsRequired();

            builder.Property(x=> x.Role)
                .HasConversion<short>()
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasConversion<int>()
                .IsRequired();

            builder.HasMany(x => x.Addresses)
                .WithOne(x => x.Person)
                .HasForeignKey(x => x.PersonId)
                .IsRequired();

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

        }
    }
    
}
