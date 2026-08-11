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

        ConfigureSeedData(builder);
    }

    private static void ConfigureSeedData(EntityTypeBuilder<FormCategory> builder) {
        builder.HasData(
                new FormCategory
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                    CategoryCode = "CONSENT",
                    CategoryName = "Consent Form",
                    CreatedAt = new DateTime(2026, 8, 11, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new FormCategory
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                    CategoryCode = "QUESTIONNAIRE",
                    CategoryName = "Questionnaire",
                    CreatedAt = new DateTime(2026, 8, 11, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new FormCategory
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                    CategoryCode = "ADMINISTRATIVE",
                    CategoryName = "Administrative Form",
                    CreatedAt = new DateTime(2026, 8, 11, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new FormCategory
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                    CategoryCode = "GUIDE",
                    CategoryName = "Patient Guide",
                    CreatedAt = new DateTime(2026, 8, 11, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                },
                new FormCategory
                {
                    Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                    CategoryCode = "OTHER",
                    CategoryName = "Other",
                    CreatedAt = new DateTime(2026, 8, 11, 0, 0, 0, DateTimeKind.Utc),
                    IsDeleted = false
                }
        );
    }
}