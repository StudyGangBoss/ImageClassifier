using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("images");

        builder.Property(image => image.Id)
            .ValueGeneratedNever();
        builder.HasKey(image => image.Id);

        builder.HasMany(image => image.ImageClassifications)
            .WithOne(ic => ic.Image);
        builder.Property(image => image.ImageData)
            .HasConversion(
                v => v == null ? null : Convert.ToBase64String(v),
                v => v == null ? null : Convert.FromBase64String(v));
    }
}