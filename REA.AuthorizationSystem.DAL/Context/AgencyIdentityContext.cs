using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.DAL.Context;

public class AgencyIdentityContext : IdentityDbContext<User>
{
    public AgencyIdentityContext(DbContextOptions<AgencyIdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
    }
}