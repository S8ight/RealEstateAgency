using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.FirstName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(255)
            .IsRequired();

        // builder.HasMany(u => u.Chat)
        //     .WithOne(c => c.User)
        //     .HasForeignKey(c => c.UserId);
    }
}