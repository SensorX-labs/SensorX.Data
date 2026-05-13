using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.CatalogContext.CategoryAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new ProductId(v))
            .ValueGeneratedNever();

        builder.Property(s => s.Code)
            .HasConversion(c => c.Value, v => Code.From(v));

        builder.Property(s => s.CategoryId)
            .HasConversion(id => id.Value, v => new CategoryId(v));

        builder.OwnsMany(s => s.Images, s =>
        {
            s.ToTable("ProductImages");
            s.WithOwner().HasForeignKey("ProductId");
            s.Property(p => p.ImageUrl).HasMaxLength(255);
        });

        builder.OwnsMany(s => s.Attributes, s =>
        {
            s.ToTable("ProductAttributes");
            s.WithOwner().HasForeignKey("ProductId");
            s.Property(p => p.AttributeName).HasMaxLength(255);
            s.Property(p => p.AttributeValue).HasMaxLength(255);
        });

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}