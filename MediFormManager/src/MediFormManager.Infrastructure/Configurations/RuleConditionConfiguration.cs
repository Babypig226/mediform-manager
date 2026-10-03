using MediFormManager.Domain.Entities.Rules;
using MediFormManager.Domain.Entities.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations
{
    public class RuleConditionConfiguration : IEntityTypeConfiguration<RuleCondition>
    {
        public void Configure(EntityTypeBuilder<RuleCondition> builder)
        {
            builder.ToTable("RuleConditions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Operator)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.ComponentRule)
                .WithMany(x => x.Conditions)   
                .HasForeignKey(x => x.ComponentRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.SourceComponent)
                .WithMany()
                .HasForeignKey(x => x.SourceComponentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ExpectedOption)
               .WithMany()
               .HasForeignKey(x => x.ExpectedOptionId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
