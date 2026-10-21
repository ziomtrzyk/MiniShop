using Catalog.API.Dtos;
using Catalog.API.Infrastructure;
using Catalog.API.Mappings;
using Catalog.API.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Apis;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog");

        api.MapGet("/items", GetAllItems);

        return app;
    }

    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetAllItems (
        [AsParameters] PaginationRequest paginationRequest,
        CatalogContext context,
        CancellationToken cancellationToken
    )
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var totalItems = await context.CatalogItems.LongCountAsync(cancellationToken);

        var itemsOnPage = await context.CatalogItems
            .AsNoTracking()
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var data = itemsOnPage.Select(i => i.ToDto()).ToList();

        return TypedResults.Ok(new PaginatedItems<CatalogItemDto>(pageIndex, pageSize, totalItems, data));
    }
}