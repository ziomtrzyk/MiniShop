using Basket.API.Model;
using Basket.API.Repositories;
using Basket.API.Services;
using Grpc.Core;

namespace Basket.API.Grpc;

public class BasketService(IBasketRepository repository, CatalogClient catalogClient) : Basket.BasketBase
{
    public override async Task<CustomerBasketResponse> GetBasket(GetBasketRequest request, ServerCallContext context)
    {
        var basket = await repository.GetBasketAsync(request.BuyerId);

        return basket is null
            ? new CustomerBasketResponse { BuyerId = request.BuyerId}
            : MapToResponse(basket);
    }

    public override async Task<CustomerBasketResponse> UpdateBasket(UpdateBasketRequest request, ServerCallContext context)
    {
        if(request.Items.Any(i => i.Quantity < 1))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Quantity must be at least 1."));
        }

        var productIds = request.Items.Select(i => i.ProductId).Distinct().ToList();
        var catalogItems = await catalogClient.GetItemsByIdsAsync(productIds, context.CancellationToken);
        var catalogItemsById = catalogItems.ToDictionary(c => c.Id);

        var missingIds = productIds.Where(id => !catalogItemsById.ContainsKey(id)).ToList();
        if(missingIds.Count > 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Products not found in catalog : {string.Join(", ", missingIds)}."));
        }

        var basket = new CustomerBasket
        {
            BuyerId = request.BuyerId,
            Items = request.Items
                .Select(i => new Model.BasketItem
                {
                    ProductId = i.ProductId,
                    ProductName = catalogItemsById[i.ProductId].Name,
                    UnitPrice = catalogItemsById[i.ProductId].Price,
                    Quantity = i.Quantity
                })
                .ToList()
        };

        var saved = await repository.UpdateBasketAsync(basket)
            ?? throw new RpcException(new Status(StatusCode.Internal, "Cannot update the basket."));

        return MapToResponse(saved);
    }

    public override async Task<DeleteBasketResponse> DeleteBasket(DeleteBasketRequest request, ServerCallContext context)
    {
        var deleted = await repository.DeleteBasketAsync(request.BuyerId);

        if(!deleted)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Basket for buyer {request.BuyerId} does not exist"));
        }

        return new DeleteBasketResponse();
    }

    private static CustomerBasketResponse MapToResponse(CustomerBasket basket)
    {
        var response = new CustomerBasketResponse { BuyerId = basket.BuyerId};

        response.Items.AddRange(basket.Items.Select(i => new BasketItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        }));

        return response;
    }
}