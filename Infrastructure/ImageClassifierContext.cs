using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Infrastructure;

public sealed class ImageClassifierContext : DbContext
{
    public DbSet<Image> Images { get; set; }
    public DbSet<ImageClassification> ImageClassifications { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<ClassificationType> ClassificationTypes { get; set; }

    public ImageClassifierContext(DbContextOptions<ImageClassifierContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("imageclassifier");

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}