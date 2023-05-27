using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ClassificationTypeConfiguration : IEntityTypeConfiguration<ClassificationType>
{
    public void Configure(EntityTypeBuilder<ClassificationType> builder)
    {
        builder.ToTable("classification_types");

        builder.Property(user => user.Id)
            .ValueGeneratedNever();
        builder.HasKey(ct => ct.Id);
        
        builder.Property(ct=>ct.Question);
        builder.Property(ct=>ct.Class);
    }
}