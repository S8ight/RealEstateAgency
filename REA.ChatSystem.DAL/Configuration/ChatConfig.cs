using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Configuration;

public class ChatConfig : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(c => c.ChatId);

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.ReceiverId)
            .IsRequired();
            
        builder.Property(c => c.Created)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.Chat)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Chat)
            .HasForeignKey(a => a.ChatId).OnDelete(DeleteBehavior.NoAction);
    }
}