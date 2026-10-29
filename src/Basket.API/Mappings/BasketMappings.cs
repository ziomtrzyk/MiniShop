using Basket.API.Dtos;
using Basket.API.Model;
using Basket.API.Services;

namespace Basket.API.Mappings;

public static class BasketMappings
{
    public static BasketItem ToBasketItem(this BasketItemDto input, CatalogItemResponse catalogItem) => new()
    {
      ProductId = input.ProductId,
      ProductName = catalogItem.Name,
      UnitPrice = catalogItem.Price,
      Quantity = input.Quantity
    };

    public static CustomerBasket ToCustomerBasket(this UpdateBasketDto input, string buyerId, Dictionary<int, CatalogItemResponse> catalogItems) => new()
    {
      BuyerId = buyerId,
      Items = input.Items.Select(b => b.ToBasketItem(catalogItems[b.ProductId])).ToList()
    };
}