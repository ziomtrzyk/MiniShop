using EventBus.Abstractions;
using EventBus.Events;

namespace EventBus.Abstractions;

public interface  IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken);

    Task IIntegrationEventHandler.Handle(IntegrationEvent @event, CancellationToken cancellationToken)
        => Handle((TIntegrationEvent)@event, cancellationToken);
}

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent @event, CancellationToken cancellationToken);
}