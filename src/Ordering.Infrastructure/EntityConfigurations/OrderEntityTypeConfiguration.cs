using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.Property(o => o.BuyerId)
            .HasMaxLength(100);

        builder.Property(o => o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.OwnsOne(o => o.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(200);
            address.Property(a => a.City).HasMaxLength(100);
            address.Property(a => a.State).HasMaxLength(100);
            address.Property(a => a.Country).HasMaxLength(100);
            address.Property(a => a.ZipCode).HasMaxLength(20);
        });

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}