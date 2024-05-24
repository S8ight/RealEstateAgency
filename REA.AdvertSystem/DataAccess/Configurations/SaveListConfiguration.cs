using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaveListConfiguration : IEntityTypeConfiguration<SaveList>
{
    public void Configure(EntityTypeBuilder<SaveList> builder)
    {
        builder.ToTable("SaveLists");

        builder.HasKey(sl => sl.Id);
        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(sl => sl.AdvertId)
            .IsRequired();

        builder.Property(sl => sl.UserId)
            .IsRequired();
        
        builder.HasOne(sl => sl.Advert) 
            .WithMany(a => a.SaveLists)
            .HasForeignKey(sl => sl.AdvertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
