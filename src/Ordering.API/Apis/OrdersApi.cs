using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Ordering.API.Application.Commands;
using Ordering.API.Application.Queries;
using Ordering.Domain.Exceptions;

namespace Ordering.API.Apis;

public static class OrdersApi
{
    public static IEndpointRouteBuilder MapOrdersApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/orders");

        api.MapPost("/", CreateOrderAsync);

        api.MapGet("/{orderId:int}", GetOrderAsync);

        api.MapGet("/", GetOrdersByBuyerAsync);

        api.MapPut("/{orderId:int}/cancel", CancelOrderAsync);

        return app;
    }

    public static async Task<Results<Created<int>, ValidationProblem, ProblemHttpResult>> CreateOrderAsync(
        CreateOrderCommand request,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var orderId = await mediator.Send(request, cancellationToken);
            return TypedResults.Created($"/api/orders/{orderId}", orderId);
        }
        catch (ValidationException ex)
        {
            return TypedResults.ValidationProblem(ToDictionaryErrors(ex));
        }

        catch (OrderingDomainException ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    public static async Task<Results<Ok<OrderDetails>, NotFound>> GetOrderAsync(
        int orderId,
        IOrderQueries orderQueries,
        CancellationToken cancellationToken
    )
    {
        var order = await orderQueries.GetOrderAsync(orderId, cancellationToken);

        return order is null ? TypedResults.NotFound() : TypedResults.Ok(order);
    }

    public static async Task<Ok<List<OrderSummary>>> GetOrdersByBuyerAsync(
        string buyerId,
        IOrderQueries orderQueries,
        CancellationToken cancellationToken
    )
    {
        return TypedResults.Ok(await orderQueries.GetOrdersByBuyerAsync(buyerId, cancellationToken));
    }

    public static async Task<Results<NoContent, NotFound, ValidationProblem, ProblemHttpResult>> CancelOrderAsync(
        int orderId,
        IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        var request = new CancelOrderCommand(orderId);
        try
        {
            return await mediator.Send(request, cancellationToken) ? TypedResults.NoContent() : TypedResults.NotFound();
        }
        catch (ValidationException ex)
        {
            return TypedResults.ValidationProblem(ToDictionaryErrors(ex));
        }
        catch (OrderingDomainException ex)
        {
            return TypedResults.Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }

    public static Dictionary<string, string[]> ToDictionaryErrors(ValidationException ex)
    {
        return ex.Errors
               .GroupBy(e => e.PropertyName)
               .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
    }
}