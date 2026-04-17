using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.CatalogContext.InternalPriceAggregate;
using SensorX.Data.Domain.Contexts.CatalogContext.ProductAggregate;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class InternalPriceConfiguration : IEntityTypeConfiguration<InternalPrice>
{
    public void Configure(EntityTypeBuilder<InternalPrice> builder)
    {
        builder.ToTable("InternalPrices");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new InternalPriceId(v))
            .ValueGeneratedNever();

        builder.Property(s => s.ProductId)
            .HasConversion(id => id.Value, v => new ProductId(v));

        builder.OwnsOne(s => s.SuggestedPrice, s =>
        {
            s.Property(p => p.Amount).HasColumnName("SuggestedPriceAmount");
            s.Property(p => p.Currency).HasColumnName("SuggestedPriceCurrency");
        });

        builder.OwnsOne(s => s.FloorPrice, s =>
        {
            s.Property(p => p.Amount).HasColumnName("FloorPriceAmount");
            s.Property(p => p.Currency).HasColumnName("FloorPriceCurrency");
        });

        builder.OwnsMany(s => s.PriceTiers, s =>
        {
            s.ToTable("PriceTiers");
            s.WithOwner().HasForeignKey("InternalPriceId");
            s.Property(p => p.Quantity).HasConversion(q => q.Value, v => new Quantity(v));
            s.OwnsOne(p => p.Price, p =>
            {
                p.Property(x => x.Amount).HasColumnName("PriceAmount");
                p.Property(x => x.Currency).HasColumnName("PriceCurrency");
            });
        });

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<InternalPrice>(p => p.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}