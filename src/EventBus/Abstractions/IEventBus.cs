using EventBus.Events;

namespace EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);    
}