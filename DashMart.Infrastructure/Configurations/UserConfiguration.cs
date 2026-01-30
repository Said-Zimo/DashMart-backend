

using DashMart.Domain.People.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("Users");

            builder.Property(x => x.Permissions)
                .IsRequired();

            builder.Property(x => x.UserName)
                .HasMaxLength(50)
                .IsRequired();


            builder.Property(x => x.RowVersion)
                .IsRowVersion();
        }

    }
}
