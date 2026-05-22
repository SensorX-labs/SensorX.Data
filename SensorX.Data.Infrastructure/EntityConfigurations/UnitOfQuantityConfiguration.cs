using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.CatalogContext.UnitOfQuantityAggregate;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class UnitOfQuantityConfiguration : IEntityTypeConfiguration<UnitOfQuantity>
{
    public void Configure(EntityTypeBuilder<UnitOfQuantity> builder)
    {
        builder.ToTable("UnitOfQuantities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, value => new UnitOfQuantityId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);
    }
}
