using MediFormManager.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class OrderFormMappingConfiguration
    : IEntityTypeConfiguration<OrderFormMapping>
{
    public void Configure(EntityTypeBuilder<OrderFormMapping> builder)
    {
        builder.ToTable("OrderFormMappings");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new
        {
            x.OrderId,
            x.FormId
        })
        .IsUnique();

        builder.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Form)
            .WithMany()
            .HasForeignKey(x => x.FormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}