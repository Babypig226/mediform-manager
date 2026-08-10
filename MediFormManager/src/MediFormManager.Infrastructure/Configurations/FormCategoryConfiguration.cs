using MediFormManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class FormCategoryConfiguration
    : IEntityTypeConfiguration<FormCategory>
{
    public void Configure(EntityTypeBuilder<FormCategory> builder)
    {
        builder.ToTable("FormCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CategoryCode)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.CategoryCode)
            .IsUnique();

        builder.Property(x => x.CategoryName)
            .HasMaxLength(100)
            .IsRequired();
    }
}