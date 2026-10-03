using MediFormManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LoginId)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.LoginId)
            .IsUnique();

        builder.Property(x => x.UserName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);//Prevent cascading delete to avoid accidental deletion of related entities

        builder.HasOne(x => x.Department)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);//Prevent cascading delete to avoid accidental deletion of related entities

        builder.HasOne(x => x.JobPosition)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.JobPositionId)
            .OnDelete(DeleteBehavior.Restrict); //Prevent cascading delete to avoid accidental deletion of related entities
    }
}