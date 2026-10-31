using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("orderItems");

        builder.Property(o => o.ProductName)
            .HasMaxLength(50);

        builder.Property(o => o.UnitPrice)
            .HasPrecision(18,2);  
    }
}