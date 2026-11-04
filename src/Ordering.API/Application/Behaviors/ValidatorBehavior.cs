using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Ordering.API.Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidatorBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var results = await validator.ValidateAsync(request, cancellationToken);
            failures.AddRange(results.Errors);
        }

        if(failures.Count > 0)
        {
            logger.LogWarning("Validation failed for {RequestName} - errors: {@ValidationErrors}",
                typeof(TRequest).Name, failures.Select(f => f.ErrorMessage));

            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}