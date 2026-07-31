using DirectoryService.Domain.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Postgres.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
  public void Configure(EntityTypeBuilder<Department> builder)
  {
    builder.ToTable("departments");
    
    builder.HasKey(x => x.Id).HasName("pk_departments");
    
    builder.Property(x => x.Id).HasColumnName("id");
    
    builder.HasIndex(x => x.Name).IsUnique();
    
    builder.Property(x => x.Name)
      .HasColumnName("name")
      .HasMaxLength(100)
      .IsRequired();
    
    builder.HasIndex(x => x.Slug).IsUnique();
    
    builder.Property(x => x.Slug)
      .HasColumnName("slug")
      .HasMaxLength(500)
      .IsRequired();
    
    builder.HasIndex(x => x.Path).IsUnique();
    
    builder.Property(x => x.Path)
      .HasColumnName("path")
      .HasMaxLength(500)
      .IsRequired();
    
    builder.Property(x => x.ParentId)
      .HasColumnName("parent_id");
    
    builder.Property(x => x.CreatedAt)
      .HasColumnName("created_at")
      .IsRequired();
    
    builder.Property(x => x.UpdatedAt)
      .HasColumnName("updated_at")
      .IsRequired();

    builder
      .HasMany(d => d.Locations)
      .WithOne()
      .HasForeignKey(d => d.DepartmentId)
      .OnDelete(DeleteBehavior.Cascade);
    
    builder
      .HasMany(d => d.Positions)
      .WithOne()
      .HasForeignKey(d => d.DepartmentId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}