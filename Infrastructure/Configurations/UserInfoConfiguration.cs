using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("users");

        builder.Property(user => user.Id)
            .ValueGeneratedNever();
       
        builder.HasIndex(u => u.ChatId);
        builder.Navigation(user => user.ImageClassifications).AutoInclude();
    }
}
public class ClassificationTypeConfiguration : IEntityTypeConfiguration<ClassificationType>
{
    public void Configure(EntityTypeBuilder<ClassificationType> builder)
    {
        builder.ToTable("users");

        builder.Property(user => user.Id)
            .ValueGeneratedNever();
    }
}