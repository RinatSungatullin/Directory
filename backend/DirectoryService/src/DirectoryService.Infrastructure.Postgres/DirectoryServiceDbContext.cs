using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.Positions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres;

public class DirectoryServiceDbContext : DbContext
{
  public DbSet<Department> Departments { get; set; }
  
  public DbSet<Location> Locations { get; set; }
  
  public DbSet<Position> Positions { get; set; }
  
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
  
  public static ILoggerFactory CreateLoggerFactory() =>
    LoggerFactory.Create(builder => builder.AddConsole());
}