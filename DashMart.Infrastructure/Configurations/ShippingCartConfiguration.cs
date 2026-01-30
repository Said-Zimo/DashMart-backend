

using DashMart.Domain.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DashMart.Infrastructure.Configurations
{
    public sealed  class ShippingCartConfiguration : EntityConfiguration<Cart>
    {

        public override void Configure(EntityTypeBuilder<Cart> builder)
        {
            base.Configure(builder);

            builder.ToTable("ShippingCarts");

            builder.Ignore(x=> x.TotalAmount);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();
        }
     
    }
}
