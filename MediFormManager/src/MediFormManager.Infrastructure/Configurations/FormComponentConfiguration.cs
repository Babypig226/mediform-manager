using MediFormManager.Domain.Entities;
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

        builder.Property(x => x.ComponentKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ComponentType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Required)
            .HasDefaultValue(false);

        builder.HasIndex(x => new
        {
            x.FormVersionId,
            x.ComponentKey
        })
        .IsUnique();

        builder.HasOne(x => x.FormVersion)
            .WithMany()
            .HasForeignKey(x => x.FormVersionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}