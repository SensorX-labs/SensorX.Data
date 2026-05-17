using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.UserContext.StaffAggregate;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staffs");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(id => id.Value, v => new StaffId(v))
            .ValueGeneratedNever();

        builder.Property(s => s.AccountId)
            .HasConversion(id => id.Value, v => new AccountId(v));

        builder.Property(s => s.Code)
            .HasConversion(c => c.Value, v => Code.From(v));

        builder.Property(s => s.Name).IsRequired();

        builder.Property(s => s.Phone)
            .HasConversion(p => p != null ? p.Value : null, v => v != null ? Phone.From(v) : null);

        builder.Property(s => s.Email)
            .HasConversion(e => e.Value, v => Email.From(v));

        builder.Property(s => s.CitizenId)
            .HasConversion(c => c != null ? c.Value : null, v => v != null ? CitizenId.From(v) : null);
        builder.Property(s => s.Department)
            .HasConversion(
                d => d.ToString(),
                v => (Department)Enum.Parse(typeof(Department), v));
    }
}