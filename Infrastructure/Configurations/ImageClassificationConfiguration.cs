using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ImageClassificationConfiguration : IEntityTypeConfiguration<ImageClassification>
{
    public void Configure(EntityTypeBuilder<ImageClassification> builder)
    {
        builder.ToTable("image_classifications");

        builder.Property(ic => ic.Id)
            .ValueGeneratedNever();
        builder
            .HasOne(ic => ic.User)
            .WithMany(u => u.ImageClassifications);
        builder.HasOne(ic => ic.ClassificationType);
        builder.Navigation(image => image.User).AutoInclude();
        builder.Navigation(image => image.Image).AutoInclude();
    }
}