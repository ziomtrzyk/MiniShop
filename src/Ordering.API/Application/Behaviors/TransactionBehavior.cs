using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Exceptions;
using Ordering.Infrastructure;

namespace Ordering.API.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(
    OrderingContext dbContext,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (dbContext.Database.CurrentTransaction is not null)
        {
            return await next(cancellationToken);
        }

        var requestName = typeof(TRequest).Name;
        var strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Begin transaction {TransactionId} for {RequestName}",
                transaction.TransactionId, requestName);

            try
            {
                var response = await next(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                logger.LogInformation("Commit transaction {TransactionId} for {RequestName}",
                    transaction.TransactionId, requestName);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Rolled back transaction {TransactionId} for {RequestName}",
                transaction.TransactionId, requestName);
                throw;
            }
        });
    }
}
