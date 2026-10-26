using System.Text.Json;
using Basket.API.Model;
using StackExchange.Redis;

namespace Basket.API.Repositories;

public class RedisBasketRepository(IConnectionMultiplexer redis, ILogger<RedisBasketRepository> logger) : IBasketRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    private static RedisKey GetBasketKey(string buyerId) => $"basket:{buyerId}";

    public async Task<CustomerBasket?> GetBasketAsync(string buyerId)
    {
        var data = await _database.StringGetAsync(GetBasketKey(buyerId));

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<CustomerBasket>(data.ToString());
    }

    public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
    {
        var json = JsonSerializer.Serialize(basket);
        var saved = await _database.StringSetAsync(GetBasketKey(basket.BuyerId), json);

        if (!saved)
        {
            logger.LogWarning("Failed to save basket for buyer {BuyerId}", basket.BuyerId);
            return null;
        }

        logger.LogInformation("Basket for buyer {BuyerId} saved", basket.BuyerId);
        return await GetBasketAsync(basket.BuyerId);
    }

    public async Task<bool> DeleteBasketAsync(string buyerId)
    {
        return await _database.KeyDeleteAsync(GetBasketKey(buyerId));
    }
}
