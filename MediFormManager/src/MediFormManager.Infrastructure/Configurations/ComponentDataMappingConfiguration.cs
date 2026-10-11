using MediFormManager.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations;

public class ComponentDataMappingConfiguration
    : IEntityTypeConfiguration<ComponentDataMapping>
{
    public void Configure(EntityTypeBuilder<ComponentDataMapping> builder)
    {
        builder.ToTable("ComponentDataMappings");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.FormComponent)
            .WithMany()
            .HasForeignKey(x => x.FormComponentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DataDictionary)
            .WithMany()
            .HasForeignKey(x => x.DataDictionaryId)
            .OnDelete(DeleteBehavior.Restrict);

        // V1: Component당 활성 Mapping 최대 1개
        builder.HasIndex(x => x.FormComponentId)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}