using System.Diagnostics;
using MediatR;

namespace Ordering.API.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Handling {RequestName} ({@Request})", requestName, request);

        var timer = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        timer.Stop();

        logger.LogInformation("Handled {RequestName} - response: {@Response} - time: {ElapsedMilliseconds} ms", requestName, response, timer.ElapsedMilliseconds);

        return response;
    }
}