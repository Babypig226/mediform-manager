using MediFormManager.Domain.Entities.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations
{
    public class ComponentOptionConfiguration : IEntityTypeConfiguration<ComponentOption>
    {
        public void Configure(EntityTypeBuilder<ComponentOption> builder)
        {
            builder.ToTable("ComponentOptions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.DisplayText)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasOne(x => x.FormComponent)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.FormComponentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
