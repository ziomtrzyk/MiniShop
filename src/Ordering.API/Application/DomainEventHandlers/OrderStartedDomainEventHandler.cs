using MediatR;
using Ordering.Domain.Events;

namespace Ordering.API.Application.DomainEventHandlers;

public class OrderStartedDomainEventHandler(ILogger<OrderStartedDomainEventHandler> logger)
    : INotificationHandler<OrderStartedDomainEvent>
{
    public Task Handle(OrderStartedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order {OrderId} was created for buyer {BuyerId}", domainEvent.Order.Id, domainEvent.Order.BuyerId);

        return Task.CompletedTask;
    }
}