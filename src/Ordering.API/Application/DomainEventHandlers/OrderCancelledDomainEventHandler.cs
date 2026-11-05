using MediatR;
using Ordering.Domain.Events;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderCancelledDomainEventHandler(ILogger<OrderCancelledDomainEventHandler> logger)
    : INotificationHandler<OrderCancelledDomainEvent>
{
    public Task Handle(OrderCancelledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order {OrderId} status changed to {OrderStatus}",
            domainEvent.Order.Id, domainEvent.Order.OrderStatus);

        return Task.CompletedTask;
    }
}