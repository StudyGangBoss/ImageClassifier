using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ClassificationTypeConfiguration : IEntityTypeConfiguration<ClassificationType>
{
    public void Configure(EntityTypeBuilder<ClassificationType> builder)
    {
        builder.ToTable("users");

        builder.Property(user => user.Id)
            .ValueGeneratedNever();
    }
}