using MediFormManager.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class FormVersionConfiguration
    : IEntityTypeConfiguration<FormVersion>
{
    public void Configure(EntityTypeBuilder<FormVersion> builder)
    {
        builder.ToTable("FormVersions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Version)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.FormId,
            x.Version
        })
        .IsUnique(); // Ensure that each form can have unique versions

        builder.HasOne(x => x.Form)
            .WithMany(x => x.Versions)
            .HasForeignKey(x => x.FormId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}