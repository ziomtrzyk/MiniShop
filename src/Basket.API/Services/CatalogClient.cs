namespace Basket.API.Services;

public class CatalogClient(HttpClient httpClient)
{
    public async Task<List<CatalogItemResponse>> GetItemsByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        var query = string.Join("&", ids.Select(id => $"ids={id}"));

        var items = await httpClient.GetFromJsonAsync<List<CatalogItemResponse>>(
            $"/api/catalog/items/by?{query}", cancellationToken
        );

        return items ?? [];
    }
}

public record CatalogItemResponse(int Id, string Name, decimal Price);