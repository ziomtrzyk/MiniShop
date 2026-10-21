using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Dtos;

public record CreateCatalogItemDto(
    [Required, MaxLength(100)] string Name,
    [MaxLength(1000)] string? Description,
    [Range(0.01, 100000)] decimal Price,
    string? PictureFileName,
    [Range(1, int.MaxValue)] int CatalogTypeId,
    [Range(1, int.MaxValue)] int CatalogBrandId,
    [Range(0, int.MaxValue)] int AvailableStock
);