using MediFormManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoleCode)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.RoleCode)
            .IsUnique();

        builder.Property(x => x.RoleName)
            .HasMaxLength(100)
            .IsRequired();
    }
}