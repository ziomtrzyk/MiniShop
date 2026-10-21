using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Infrastructure;

public static class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext context, ILogger logger, CancellationToken cancellationToken = default)
    {
        if(await context.CatalogItems.AnyAsync(cancellationToken))
        {
            return;
        }

        var daybird = new CatalogBrand { Brand = "Daybird"};
        var gravitator = new CatalogBrand { Brand = "Gravitator"};
        var wildRunner = new CatalogBrand { Brand = "WildRunner"};

        var footwear = new CatalogType { Type = "Footwear" };
        var jackets = new CatalogType { Type = "Jackets" };
        var bags = new CatalogType { Type = "Bags" };

        CatalogItem[] items =
        [
            new() { Name = "Wanderer Black Hiking Boots", Price = 109.99m, AvailableStock = 100, CatalogBrand = daybird, CatalogType = footwear },
            new() { Name = "Vertical Journey Climbing Shoes", Price = 129.99m, AvailableStock = 100, CatalogBrand = gravitator, CatalogType = footwear },
            new() { Name = "Trail Runner Sneakers", Price = 89.99m, AvailableStock = 100, CatalogBrand = wildRunner, CatalogType = footwear },
            new() { Name = "Alpine Winter Boots", Price = 149.99m, AvailableStock = 100, CatalogBrand = gravitator, CatalogType = footwear },
            new() { Name = "Frostbite Insulated Jacket", Price = 179.99m, AvailableStock = 100, CatalogBrand = daybird, CatalogType = jackets },
            new() { Name = "Summit Pro Insulated Jacket", Price = 199.99m, AvailableStock = 100, CatalogBrand = gravitator, CatalogType = jackets },
            new() { Name = "Rainshield Shell Jacket", Price = 119.99m, AvailableStock = 100, CatalogBrand = wildRunner, CatalogType = jackets },
            new() { Name = "Featherlight Down Vest", Price = 79.99m, AvailableStock = 100, CatalogBrand = daybird, CatalogType = jackets },
            new() { Name = "Expedition Backpack", Price = 139.99m, AvailableStock = 100, CatalogBrand = gravitator, CatalogType = bags },
            new() { Name = "Explorer 45L Backpack", Price = 99.99m, AvailableStock = 100, CatalogBrand = wildRunner, CatalogType = bags },
            new() { Name = "Stellar Duffle Bag", Price = 69.99m, AvailableStock = 100, CatalogBrand = daybird, CatalogType = bags },
            new() { Name = "Urban Daypack", Price = 49.99m, AvailableStock = 100, CatalogBrand = wildRunner, CatalogType = bags },
        ];

        context.CatalogItems.AddRange(items);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Seeded catalog with {ItemCount} items", items.Length);
    }
}