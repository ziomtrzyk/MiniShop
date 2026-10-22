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
    public static CatalogItem ToCatalogItem(this CreateCatalogItemDto input) => new()
    {
        Name = input.Name,
        Description = input.Description,
        Price = input.Price,
        PictureFileName = input.PictureFileName,
        CatalogTypeId = input.CatalogTypeId,
        CatalogBrandId = input.CatalogBrandId,
        AvailableStock = input.AvailableStock
    };
    public static void UpdateCatalogItem(this CatalogItem catalogItem, CreateCatalogItemDto input)
    {
        catalogItem.Name = input.Name;
        catalogItem.Description = input.Description;
        catalogItem.Price = input.Price;
        catalogItem.PictureFileName = input.PictureFileName;
        catalogItem.CatalogTypeId = input.CatalogTypeId;
        catalogItem.CatalogBrandId = input.CatalogBrandId;
        catalogItem.AvailableStock = input.AvailableStock;
    }
}