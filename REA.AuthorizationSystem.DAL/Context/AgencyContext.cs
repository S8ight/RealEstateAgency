using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using REA.AuthorizationSystem.DAL.Entities;

namespace REA.AuthorizationSystem.DAL.Context;

public class AgencyContext : IdentityDbContext<User>
{
    public AgencyContext(DbContextOptions<AgencyContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
    }
}