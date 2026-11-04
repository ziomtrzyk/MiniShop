using FluentValidation;
using Ordering.API.Application.Commands;

namespace Ordering.API.Application.Validations;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.BuyerId).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Street).NotEmpty().MaximumLength(200);
        RuleFor(c => c.City).NotEmpty().MaximumLength(100);
        RuleFor(c => c.State).MaximumLength(100);
        RuleFor(c => c.Country).NotEmpty().MaximumLength(100);
        RuleFor(c => c.ZipCode).NotEmpty().MaximumLength(20);

        RuleFor(c => c.Items).NotEmpty().WithMessage("An order must contain at least one item.");

        RuleForEach(c => c.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).GreaterThan(0);
            item.RuleFor(i => i.ProductName).NotEmpty().MaximumLength(50);
            item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
            item.RuleFor(i => i.Units).InclusiveBetween(1,100);
        });
    }
}