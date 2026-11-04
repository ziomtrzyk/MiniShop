using FluentValidation;
using Ordering.API.Application.Commands;

namespace Ordering.API.Application.Validations;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(c => c.OrderId).GreaterThan(0);
    }
}