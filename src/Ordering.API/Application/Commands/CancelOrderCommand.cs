using MediatR;

namespace Ordering.API.Application.Commands;

public record CancelOrderCommand(
    int OrderId
) : IRequest<bool>;