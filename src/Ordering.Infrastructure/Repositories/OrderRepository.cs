using Microsoft.EntityFrameworkCore;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderingContext context) : IOrderRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Order Add(Order order)
    {
        return context.Orders.Add(order).Entity;
    }

    public async Task<Order?> GetAsync(int orderId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
}
