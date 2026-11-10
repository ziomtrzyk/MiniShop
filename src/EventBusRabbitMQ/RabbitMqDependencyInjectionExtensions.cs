using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class RabbitMqDependencyInjectionExtensions
{
    public static IHostApplicationBuilder AddRabbitMqEventBus(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddRabbitMQClient(connectionName);

        builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

        return builder;
    }
}