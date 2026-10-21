using Catalog.API.Dtos;
using Catalog.API.Infrastructure;
using Catalog.API.Mappings;
using Catalog.API.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Apis;

public static class CatalogApi
{
    public static IEndpointRouteBuilder MapCatalogApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/catalog");

        api.MapGet("/items", GetAllItems);

        api.MapGet("/items/{id:int}", GetItemById);

        api.MapPost("/items", CreateItem);

        return app;
    }

    public static async Task<Ok<PaginatedItems<CatalogItemDto>>> GetAllItems(
        [AsParameters] PaginationRequest paginationRequest,
        CatalogContext context,
        int? brand,
        int? type,
        CancellationToken cancellationToken
    )
    {
        var pageSize = paginationRequest.PageSize;
        var pageIndex = paginationRequest.PageIndex;

        var root = context.CatalogItems.AsQueryable();

        if (brand is not null)
        {
            root = root.Where(c => c.CatalogBrandId == brand);
        }
        if (type is not null)
        {
            root = root.Where(c => c.CatalogTypeId == type);
        }

        var totalItems = await root.LongCountAsync(cancellationToken);

        var itemsOnPage = await root
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

    public static async Task<Results<Ok<CatalogItemDto>, NotFound>> GetItemById(
        int id,
        CatalogContext context,
        CancellationToken cancellationToken
    )
    {
        var item = await context.CatalogItems
            .AsNoTracking()
            .Include(c => c.CatalogBrand)
            .Include(c => c.CatalogType)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return item is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(item.ToDto());
    }

    public static async Task<Results<Created<CatalogItemDto>, ValidationProblem>> CreateItem(
        CreateCatalogItemDto dto,
        CatalogContext context,
        CancellationToken cancellationToken)
    {
        var validations = new Dictionary<string, string[]>();
        if (!await context.CatalogBrands.AnyAsync(b => b.Id == dto.CatalogBrandId, cancellationToken))
        {
            validations.Add("CatalogBrandId", ["Brand does not exist."]);
        }
        if (!await context.CatalogTypes.AnyAsync(t => t.Id == dto.CatalogTypeId, cancellationToken))
        {
            validations.Add("CatalogTypeId", ["Type does not exist."]);
        }

        if (validations.Count > 0)
            return TypedResults.ValidationProblem(validations);
            
        var item = dto.ToCatalogItem();

        context.CatalogItems.Add(item);
        await context.SaveChangesAsync(cancellationToken);

        await context.Entry(item).Reference(i => i.CatalogBrand).LoadAsync(cancellationToken);
        await context.Entry(item).Reference(i => i.CatalogType).LoadAsync(cancellationToken);

        return TypedResults.Created($"/api/catalog/items/{item.Id}", item.ToDto());
    }
}