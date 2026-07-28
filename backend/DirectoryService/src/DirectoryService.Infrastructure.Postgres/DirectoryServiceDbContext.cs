using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DirectoryService.Infrastructure.Postgres;

public class DirectoryServiceDbContext : DbContext
{
  public DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options)
    : base(options)
  { }
}