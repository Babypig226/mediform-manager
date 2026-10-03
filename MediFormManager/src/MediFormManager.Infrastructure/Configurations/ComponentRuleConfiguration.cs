using MediFormManager.Domain.Entities.Rules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations
{
    public class ComponentRuleConfiguration : IEntityTypeConfiguration<ComponentRule>
    {
        public void Configure(EntityTypeBuilder<ComponentRule> builder)
        {
            builder.ToTable("ComponentRules");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.RuleName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Logic)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.FormVersion)
                .WithMany(x => x.Rules)
                .HasForeignKey(x => x.FormVersionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
