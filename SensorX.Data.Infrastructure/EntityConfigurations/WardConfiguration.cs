using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class WardConfiguration : IEntityTypeConfiguration<Ward>
{
    public void Configure(EntityTypeBuilder<Ward> builder)
    {
        builder.ToTable("Wards");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasConversion(id => id.Value, v => new WardId(v))
            .ValueGeneratedNever();

        builder.Property(w => w.Name).IsRequired();

        builder.Property(w => w.ProvinceId)
            .HasConversion(id => id.Value, v => new ProvinceId(v))
            .IsRequired();

        builder.HasOne(w => w.Province)
            .WithMany()
            .HasForeignKey(w => w.ProvinceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}