using MediFormManager.Domain.Entities.Rules;
using MediFormManager.Domain.Entities.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations
{
    public class RuleActionConfiguration : IEntityTypeConfiguration<RuleAction>
    {
        public void Configure(EntityTypeBuilder<RuleAction> builder)
        {
            builder.ToTable("RuleActions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ActionType)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(x => x.TargetType)
                .HasConversion<string>()
                .IsRequired();
            builder.Property(x => x.TargetGroupKey)
                .HasMaxLength(100);
            builder.HasOne(x => x.ComponentRule)
                .WithMany(x => x.Actions)
                .HasForeignKey(x => x.ComponentRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.TargetComponent)
                .WithMany()
                .HasForeignKey(x => x.TargetComponentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
