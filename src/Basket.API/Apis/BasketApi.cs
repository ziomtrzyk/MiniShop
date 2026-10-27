using Basket.API.Dtos;
using Basket.API.Mappings;
using Basket.API.Model;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Basket.API.Apis;

public static class BasketApi
{
    public static IEndpointRouteBuilder MapBasketApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/basket");

        api.MapGet("/{buyerId}", GetBasketById);

        api.MapPut("/{buyerId}", UpdateBasket);

        api.MapDelete("/{buyerId}", DeleteBasket);

        return app;
    }

    public static async Task<Ok<CustomerBasket>> GetBasketById(
        string buyerId,
        IBasketRepository basketRepository
    )
    {
        var basket = await basketRepository.GetBasketAsync(buyerId);

        if(basket is null)
        {
            return TypedResults.Ok(new CustomerBasket{BuyerId = buyerId});
        }
        
        return TypedResults.Ok(basket);
    }

    public static async Task<Results<Ok<CustomerBasket>, ProblemHttpResult>> UpdateBasket(
        string buyerId,
        UpdateBasketDto updateBasketDto,
        IBasketRepository basketRepository
    )
    {
        var basket = updateBasketDto.ToCustomerBasket(buyerId);
        
        var basketUpdated = await basketRepository.UpdateBasketAsync(basket);

        if(basketUpdated is null)
        {
            return TypedResults.Problem("Cannot update the basket.");
        }
        return TypedResults.Ok(basketUpdated);
    }
    
    public static async Task<Results<NoContent, NotFound>> DeleteBasket(
        string buyerId,
        IBasketRepository basketRepository
    )
    {
        return await basketRepository.DeleteBasketAsync(buyerId) ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}