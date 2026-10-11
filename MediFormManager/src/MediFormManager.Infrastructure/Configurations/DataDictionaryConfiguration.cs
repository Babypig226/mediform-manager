using MediFormManager.Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediFormManager.Infrastructure.Configurations;

public class DataDictionaryConfiguration
    : IEntityTypeConfiguration<DataDictionary>
{
    public void Configure(EntityTypeBuilder<DataDictionary> builder)
    {
        builder.ToTable("DataDictionaries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DataKey)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.DisplayName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DataType)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.DataKey)
            .IsUnique();
    }
}