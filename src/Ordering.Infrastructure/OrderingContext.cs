using Microsoft.EntityFrameworkCore;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure.EntityConfigurations;

namespace Ordering.Infrastructure;

public class OrderingContext(DbContextOptions<OrderingContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }
}