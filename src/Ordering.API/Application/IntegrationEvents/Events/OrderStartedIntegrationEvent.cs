using EventBus.Events;

namespace Ordering.API.Application.IntegrationEvents.Events;

public record OrderStartedIntegrationEvent(string BuyerId, int OrderId) : IntegrationEvent;