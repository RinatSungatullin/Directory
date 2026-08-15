using System.Data;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Locations;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class EfCoreLocationsRepository : ILocationsRepository
{
  private readonly DirectoryServiceDbContext _dbContext;

  public EfCoreLocationsRepository(DirectoryServiceDbContext dbContext)
  {
    this._dbContext = dbContext;
  }
  
  public async Task<Guid> AddAsync(Location location, CancellationToken cancellationToken = default)
  {
    await this._dbContext.AddAsync(location, cancellationToken);
    
    await this._dbContext.SaveChangesAsync(cancellationToken);
    
    return location.Id;
  }

  public Task<Location> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public Task<Guid> UpdateAsync(Location location, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Guid?> GetLocationByName(string name, CancellationToken cancellationToken = default)
  {
    var locationId = await this._dbContext.Set<Location>()
      .Where(l => l.Name == name)
      .Select(l => (Guid?)l.Id)
      .FirstOrDefaultAsync(cancellationToken);

    return locationId;
  }
}