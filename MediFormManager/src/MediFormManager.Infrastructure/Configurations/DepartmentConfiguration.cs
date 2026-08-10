using MediFormManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration
    : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DepartmentCode)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.DepartmentCode)
            .IsUnique();

        builder.Property(x => x.DepartmentName)
            .HasMaxLength(100)
            .IsRequired();
    }
}