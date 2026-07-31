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
    
    builder.HasIndex(l => l.Address).IsUnique();
    
    builder.Property(l => l.Address)
      .HasColumnName("address")
      .HasMaxLength(100);

    builder.Property(l => l.CreatedAt)
      .HasColumnName("created_at");
    
    builder.Property(l => l.UpdatedAt)
      .HasColumnName("updated_at");
  }
}