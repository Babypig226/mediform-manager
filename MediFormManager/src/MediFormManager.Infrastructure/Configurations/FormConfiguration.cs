using MediFormManager.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class FormConfiguration : IEntityTypeConfiguration<Form>
{
    public void Configure(EntityTypeBuilder<Form> builder)
    {
        builder.ToTable("Forms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FormCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.FormCode)
            .IsUnique();

        builder.Property(x => x.FormName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Forms)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}