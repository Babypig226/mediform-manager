using MediFormManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class MedicalOrderConfiguration
    : IEntityTypeConfiguration<MedicalOrder>
{
    public void Configure(EntityTypeBuilder<MedicalOrder> builder)
    {
        builder.ToTable("MedicalOrders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.OrderCode)
            .IsUnique();

        builder.Property(x => x.OrderName)
            .HasMaxLength(200)
            .IsRequired();
    }
}