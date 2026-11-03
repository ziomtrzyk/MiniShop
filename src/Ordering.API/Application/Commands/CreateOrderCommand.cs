using MediatR;

namespace Ordering.API.Application.Commands;

public record CreateOrderCommand(
    string BuyerId,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    List<OrderItemDto> Items
) : IRequest<int>;

public record OrderItemDto(int ProductId, string ProductName, decimal UnitPrice, int Units);