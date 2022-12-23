using Microsoft.EntityFrameworkCore;
using REA.ChatSystem.DAL.Configuration;
using REA.ChatSystem.DAL.Models;

namespace REA.ChatSystem.DAL.Context;

public class AgencyContext : DbContext
{

    public DbSet<Chat> Chat { get; set; }

    public DbSet<Message> Message { get; set; }

    public DbSet<User> User { get; set; }

    public AgencyContext(DbContextOptions<AgencyContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ChatConfig());
        modelBuilder.ApplyConfiguration(new MessageConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());

    }
}