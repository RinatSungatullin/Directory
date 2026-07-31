using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
{
  public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
  {
    builder.ToTable("department_locations");
    
    builder.HasKey(dl => dl.Id).HasName("pk_department_locations");
    
    builder.Property(dl => dl.Id).HasColumnName("id");
    
    builder.Property(dl => dl.DepartmentId)
      .HasColumnName("department_id")
      .IsRequired();

    builder.HasOne<Department>()
      .WithMany(dl => dl.Locations)
      .HasForeignKey(dl => dl.DepartmentId);
    
    builder.HasOne<Location>()
      .WithMany()
      .HasForeignKey(dl => dl.LocationId);
  }
}