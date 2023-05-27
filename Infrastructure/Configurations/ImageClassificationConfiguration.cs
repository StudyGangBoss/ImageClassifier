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
        builder.HasKey(ic => ic.Id);

        builder.Property(ic => ic.UserId);
        builder.Property(ic => ic.Mark);
        builder.HasIndex(ic => ic.UserId);

        builder
            .HasOne(ic => ic.User)
            .WithMany(u => u.ImageClassifications);
        builder.HasOne(ic => ic.ClassificationType);
        builder.HasOne(ic => ic.Image);
        builder.Navigation(ic => ic.ClassificationType).AutoInclude();
    }
}