using Basket.API.Dtos;
using Basket.API.Model;

namespace Basket.API.Mappings;

public static class BasketMappings
{
    public static BasketItem ToBasketItem(this BasketItemDto input) => new()
    {
      ProductId = input.ProductId,
      ProductName = input.ProductName,
      UnitPrice = input.UnitPrice,
      Quantity = input.Quantity
    };

    public static CustomerBasket ToCustomerBasket(this UpdateBasketDto input, string buyerId) => new()
    {
      BuyerId = buyerId,
      Items = input.Items.Select(b => b.ToBasketItem()).ToList()
    };
}