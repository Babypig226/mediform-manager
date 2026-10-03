using MediFormManager.Domain.Entities.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class FormComponentConfiguration
    : IEntityTypeConfiguration<FormComponent>
{
    public void Configure(EntityTypeBuilder<FormComponent> builder)
    {
        builder.ToTable("FormComponents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ComponentType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Label)            
            .HasMaxLength(200);
        builder.Property(x => x.Placeholder)
            .HasMaxLength(200);
        builder.Property(x => x.GroupKey)
            .HasMaxLength(100);

        builder.Property(x => x.IsRequired)
            .HasDefaultValue(false);
        builder.Property(x => x.IsDisabled)
            .HasDefaultValue(false);
        builder.Property(x => x.IsVisible)
            .HasDefaultValue(true);

        builder.HasOne(x => x.FormVersion)
            .WithMany(x => x.Components)
            .HasForeignKey(x => x.FormVersionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}