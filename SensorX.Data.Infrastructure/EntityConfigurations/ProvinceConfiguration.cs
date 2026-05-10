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

        builder.OwnsMany(p => p.Wards, wardBuilder =>
        {
            wardBuilder.ToTable("Wards");
            wardBuilder.WithOwner().HasForeignKey("ProvinceId");

            wardBuilder.Property<ProvinceId>("ProvinceId")
                .HasConversion(id => id.Value, v => new ProvinceId(v))
                .IsRequired();

            wardBuilder.HasKey(w => w.Id);
            wardBuilder.Property(w => w.Id)
                .HasConversion(id => id.Value, v => new WardId(v))
                .ValueGeneratedNever();
        });

    }
}