using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PhotoListConfiguration : IEntityTypeConfiguration<PhotoList>
{
    public void Configure(EntityTypeBuilder<PhotoList> builder)
    {
        builder.ToTable("PhotoLists");

        builder.HasKey(pl => pl.Id);
        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(pl => pl.AdvertId)
            .IsRequired();

        builder.Property(pl => pl.PhotoUrl)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.HasOne(pl => pl.Advert)
            .WithMany(a => a.PhotoList)
            .HasForeignKey(pl => pl.AdvertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
