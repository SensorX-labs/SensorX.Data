using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Provinces");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, v => new ProvinceId(v))
            .ValueGeneratedNever();

        builder.Property(p => p.Name).IsRequired();

        builder.OwnsMany(p => p.Wards, ward =>
        {
            ward.ToTable("Wards");

            ward.HasKey(w => w.Id);

            ward.Property(w => w.Id)
                .HasConversion(id => id.Value, v => new WardId(v))
                .ValueGeneratedNever();

            ward.Property(w => w.Name).IsRequired();
        });
    }
}