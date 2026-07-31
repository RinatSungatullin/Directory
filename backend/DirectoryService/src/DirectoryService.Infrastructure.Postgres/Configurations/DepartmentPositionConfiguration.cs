using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
  public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
  {
    builder.ToTable("department_positions");
    
    builder.HasKey(p => p.Id).HasName("pk_department_positions");
    
    builder.Property(p => p.Id).HasColumnName("id");
    
    builder.Property(p => p.DepartmentId).HasColumnName("department_id");
    
    builder.Property(p => p.PositionId).HasColumnName("position_id");

    builder
      .HasOne<Department>()
      .WithMany(d => d.Positions)
      .HasForeignKey(d => d.DepartmentId);
    
    builder.HasOne<Position>()
      .WithMany()
      .HasForeignKey(d => d.PositionId);
  }
}