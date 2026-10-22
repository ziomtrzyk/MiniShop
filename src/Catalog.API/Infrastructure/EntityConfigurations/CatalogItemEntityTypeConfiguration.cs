using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Infrastructure.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.Property(i => i.Name)
            .HasMaxLength(100);

        builder.Property(i => i.Description)
            .HasMaxLength(1000);

        builder.Property(i => i.Price)
            .HasPrecision(18,2);

        builder.HasIndex(i => i.Name);

        builder
            .HasOne(i => i.CatalogBrand)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(i => i.CatalogType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}