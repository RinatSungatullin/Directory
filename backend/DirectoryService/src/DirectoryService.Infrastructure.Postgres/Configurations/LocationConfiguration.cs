using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
  public void Configure(EntityTypeBuilder<Location> builder)
  {
    builder.ToTable("locations");
    
    builder.HasKey(l => l.Id).HasName("pk_locations");
    
    builder.Property(l => l.Id)
      .HasColumnName("id");
    
    builder.HasIndex(l => l.Name).IsUnique();
    
    builder.Property(l => l.Name)
      .HasColumnName("name")
      .HasMaxLength(100);

    builder.OwnsOne(l => l.Address, a =>
    {
      a.Property(l => l.City)
        .HasColumnName("city")
        .HasMaxLength(100)
        .IsRequired();
      
      a.Property(l => l.Street)
        .HasColumnName("street")
        .HasMaxLength(100)
        .IsRequired();
      
      a.Property(l => l.Building)
        .HasColumnName("building")
        .HasMaxLength(100)
        .IsRequired();
      
      a.Property(l => l.OfficeNumber)
        .HasColumnName("office_number")
        .HasMaxLength(5)
        .IsRequired();
    });

    builder.Navigation(l => l.Address).IsRequired();

    builder.Property(l => l.CreatedAt)
      .HasColumnName("created_at");
    
    builder.Property(l => l.UpdatedAt)
      .HasColumnName("updated_at");
  }
}