using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure;

namespace Ordering.API.Application.Queries;

public class OrderQueries(OrderingContext context) : IOrderQueries
{
    public async Task<OrderDetails?> GetOrderAsync(int orderId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
                .Where(o => o.Id == orderId)
                .Select(o => new OrderDetails(
                    o.Id,
                    o.OrderDate,
                    o.OrderStatus.ToString(),
                    o.Address.Street,
                    o.Address.City,
                    o.Address.State,
                    o.Address.Country,
                    o.Address.ZipCode,
                    o.OrderItems
                        .Select(i => new OrderItemSummary(i.ProductName, i.UnitPrice, i.Units))
                        .ToList(),
                    o.OrderItems.Sum(i => i.UnitPrice * i.Units)))
                .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<OrderSummary>> GetOrdersByBuyerAsync(string buyerId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
                .Where(o => o.BuyerId == buyerId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderSummary(
                    o.Id,
                    o.OrderDate,
                    o.OrderStatus.ToString(),
                    o.OrderItems.Sum(i => i.UnitPrice * i.Units)
                    ))
                .ToListAsync(cancellationToken);
    }
}