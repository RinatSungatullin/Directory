using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Postgres;

public class DirectoryServiceDbContext : DbContext
{
  public DbSet<Department> Department { get; set; }
  
  public DbSet<Location> Location { get; set; }
  
  public DbSet<Position> Position { get; set; }
  
  public DbSet<DepartmentLocation> DepartmentLocation { get; set; }
  
  public DbSet<DepartmentPosition> DepartmentPosition { get; set; }
  
  public DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options)
    : base(options)
  { }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(
      typeof(DirectoryServiceDbContext).Assembly);

    base.OnModelCreating(modelBuilder);
  }
}