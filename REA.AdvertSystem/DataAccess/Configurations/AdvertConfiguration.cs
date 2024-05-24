using REA.AdvertSystem.DataAccess.Entities;

namespace REA.AdvertSystem.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
{
    public void Configure(EntityTypeBuilder<Advert> builder)
    {
        builder.ToTable("Adverts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Province)
            .HasMaxLength(100);

        builder.Property(a => a.Settlement)
            .HasMaxLength(100);

        builder.Property(a => a.Infrastructure)
            .HasMaxLength(500);

        builder.Property(a => a.HouseholdAppliances)
            .HasMaxLength(500);

        builder.Property(a => a.Square)
            .HasColumnType("float");

        builder.Property(a => a.FloorsNumber)
            .HasColumnType("integer");

        builder.Property(a => a.RoomsNumber)
            .HasColumnType("integer");

        builder.Property(a => a.Price)
            .HasColumnType("float");

        builder.Property(a => a.Created)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.EstateType)
            .HasConversion<string>()
            .IsRequired();
    }
}
