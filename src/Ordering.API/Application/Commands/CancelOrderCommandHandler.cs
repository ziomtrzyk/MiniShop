using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Commands;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    ILogger<CancelOrderCommandHandler> logger
) : IRequestHandler<CancelOrderCommand, bool>
{
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId, cancellationToken);

        if(order is null)
        {
            return false;
        }

        order.SetCancelledStatus();
        await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order {OrderId} Status has been changed to {OrderStatus}", order.Id, order.OrderStatus);

        return true;
    }
}