
using DashMart.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed class OrderStatusLogConfiguration : EntityConfiguration<OrderStatusLog>
    {
        public override void Configure(EntityTypeBuilder<OrderStatusLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("OrderStatusLogs");

            builder.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

        }


    }
}
