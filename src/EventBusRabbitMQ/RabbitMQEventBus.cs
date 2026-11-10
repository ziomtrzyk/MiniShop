using System.Text.Json;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBusRabbitMQ;

public sealed class RabbitMQEventBus(IConnection connection, ILogger<RabbitMQEventBus> logger) : IEventBus
{
    public const string ExchangeName = "minishop_event_bus";

    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var routingKey = @event.GetType().Name;

        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Direct,
            cancellationToken: cancellationToken
        );

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        var properties = new BasicProperties { DeliveryMode = DeliveryModes.Persistent };

        await channel.BasicPublishAsync(
            exchange: ExchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken
        );

        logger.LogInformation("Published integration event {EventId} ({EventName})", @event.Id, routingKey);
    }
}