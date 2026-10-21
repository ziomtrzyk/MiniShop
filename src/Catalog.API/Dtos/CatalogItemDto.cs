namespace Catalog.API.Dtos;

public record CatalogItemDto(int Id, string Name, string? Description, decimal Price, string? PictureFileName, int CatalogTypeId,
    string? Type, int CatalogBrandId, string? Brand, int AvailableStock);
