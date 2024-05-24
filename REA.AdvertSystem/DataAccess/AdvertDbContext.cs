using Microsoft.EntityFrameworkCore;
using REA.AdvertSystem.DataAccess.Configurations;
using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.DataAccess;

public class AdvertDbContext : DbContext
{
    public DbSet<Advert> Adverts { get; set; }
    public DbSet<PhotoList> PhotoLists { get; set; }
    public DbSet<SaveList> SaveLists { get; set; }
    public DbSet<User?> Users { get; set; }

    public AdvertDbContext(DbContextOptions<AdvertDbContext> options)
        : base(options)
    {
    }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AdvertConfiguration());
        modelBuilder.ApplyConfiguration(new PhotoListConfiguration());
        modelBuilder.ApplyConfiguration(new SaveListConfiguration());
    }
}
