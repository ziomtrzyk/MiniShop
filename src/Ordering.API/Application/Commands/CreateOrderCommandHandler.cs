using EventBus.Abstractions;
using MediatR;
using Ordering.API.Application.IntegrationEvents.Events;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Commands;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IEventBus eventBus) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.State, request.Country, request.ZipCode);
        var order = new Order(request.BuyerId, address);

        foreach (var item in request.Items)
        {
            order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Units);
        }

        orderRepository.Add(order);
        await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        var integrationEvent = new OrderStartedIntegrationEvent(order.BuyerId, order.Id);
        await eventBus.PublishAsync(integrationEvent, cancellationToken);

        return order.Id;
    }
}
