using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.SurName).HasMaxLength(100);
        builder.Property(u => u.UserName).HasMaxLength(100);
        builder.Property(u => u.Email).HasMaxLength(255);
        builder.Property(u => u.Description).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.UserName).IsUnique();
    }
}