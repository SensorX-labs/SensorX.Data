using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductCategoryAggregate;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new ProductCategoryId(v))
            .ValueGeneratedNever();

        // Configure Parent relationship
        builder.HasOne(s => s.Parent)
            .WithMany()
            .HasForeignKey(s => s.ParentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}