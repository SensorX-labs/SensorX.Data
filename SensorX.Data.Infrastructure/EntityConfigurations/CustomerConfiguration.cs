using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SensorX.Data.Domain.Contexts.UserContext.CustomerAggregate;
using SensorX.Data.Domain.Contexts.UserContext.ProvinceAggregate;
using SensorX.Data.Domain.StrongIDs;
using SensorX.Data.Domain.ValueObjects;

namespace SensorX.Data.Infrastructure.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => new CustomerId(v))
            .ValueGeneratedNever();

        builder.Property(c => c.AccountId)
            .HasConversion(id => id.Value, v => new AccountId(v));

        builder.Property(c => c.Code)
            .HasConversion(c => c.Value, v => Code.From(v));

        builder.Property(c => c.Name).IsRequired();

        builder.Property(c => c.Phone)
            .HasConversion(p => p.Value, v => Phone.From(v));

        builder.Property(c => c.Email)
            .HasConversion(e => e.Value, v => Email.From(v));

        builder.OwnsOne(c => c.ShippingInfo, s =>
        {
            s.Property(p => p.WardId)
                .HasConversion(id => id.Value, v => new WardId(v))
                .HasColumnName("WardId");

            s.Property(p => p.ShippingAddress)
                .HasColumnName("ShippingAddress");

            s.Property(p => p.ReceiverName)
                .HasColumnName("ReceiverName");

            s.Property(p => p.ReceiverPhone)
                .HasConversion(p => p.Value, v => Phone.From(v))
                .HasColumnName("ReceiverPhone");

            s.HasOne<Ward>()
                .WithMany()
                .HasForeignKey(p => p.WardId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}