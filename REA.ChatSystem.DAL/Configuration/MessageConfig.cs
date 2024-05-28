using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Configuration;

public class MessageConfig : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.MessageId);

        builder.Property(m => m.ChatId)
            .IsRequired();

        builder.Property(m => m.SenderId)
            .IsRequired();

        builder.Property(m => m.ReceiverId)
            .IsRequired();

        builder.Property(m => m.MessageBody)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(m => m.Created)
            .HasColumnType("datetime")
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(m => m.Checked)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId);
    }
}