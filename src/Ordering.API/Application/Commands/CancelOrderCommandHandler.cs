using MediatR;
using Ordering.Domain.OrderAggregate;

namespace Ordering.API.Application.Commands;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository
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
        await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}