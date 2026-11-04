namespace Ordering.API.Application.Queries;

public interface IOrderQueries
{
    Task<OrderDetails?> GetOrderAsync(int orderId, CancellationToken cancellationToken);

    Task<List<OrderSummary>> GetOrdersByBuyerAsync(string buyerId, CancellationToken cancellationToken);
}