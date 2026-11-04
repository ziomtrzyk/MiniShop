namespace Ordering.API.Application.Queries;

public record OrderDetails(
    int OrderNumber,
    DateTime Date,
    string Status,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    List<OrderItemSummary> Items,
    decimal Total);

public record OrderItemSummary(string ProductName, decimal UnitPrice, int Units);

public record OrderSummary(int OrderNumber, DateTime Date, string Status, decimal Total);
