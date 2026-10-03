using MediFormManager.Domain.Entities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Persistence.Configurations
{
    public class JobPositionConfiguration : IEntityTypeConfiguration<JobPosition>
    {
        public void Configure(EntityTypeBuilder<JobPosition> builder)
        {
            builder.ToTable("JobPositions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.JobCode)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(x => x.JobCode)
                .IsUnique();

            builder.Property(x => x.JobName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
