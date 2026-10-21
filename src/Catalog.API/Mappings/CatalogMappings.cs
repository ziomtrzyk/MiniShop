using Catalog.API.Dtos;
using Catalog.API.Model;

namespace Catalog.API.Mappings;

public static class CatalogMappings
{
    public static CatalogItemDto ToDto(this CatalogItem input) => new(
        Id: input.Id,
        Name: input.Name,
        Description: input.Description,
        Price: input.Price,
        PictureFileName: input.PictureFileName,
        CatalogTypeId: input.CatalogTypeId,
        Type: input.CatalogType?.Type,
        CatalogBrandId: input.CatalogBrandId,
        Brand: input.CatalogBrand?.Brand,
        AvailableStock: input.AvailableStock
    );
}