using AuthorizationService.DAL.Configuration;
using AuthorizationService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthorizationService.DAL.Context;

public class AuthorizationContext : DbContext
{
    public DbSet<User> User { get; set; }
    //public DbSet<RefreshToken> RefreshToken { get; set; }
    public AuthorizationContext(DbContextOptions<AuthorizationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}