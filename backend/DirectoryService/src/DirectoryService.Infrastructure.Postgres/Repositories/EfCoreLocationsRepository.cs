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

  public async Task<IEnumerable<Location>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    return await this._dbContext.Set<Location>().ToListAsync(cancellationToken);
  }

  public async Task<Location?> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    Location? location = await this._dbContext.Set<Location>()
      .FirstOrDefaultAsync(
        l => l.Id == locationId,
        cancellationToken);

    return location;
  }

  public Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Guid?> GetByName(string name, CancellationToken cancellationToken = default)
  {
    var locationId = await this._dbContext.Set<Location>()
      .Where(l => l.Name == name)
      .Select(l => (Guid?)l.Id)
      .FirstOrDefaultAsync(cancellationToken);

    return locationId;
  }

  public async Task<IEnumerable<Location>> GetByIdListAsync(
    IEnumerable<Guid> locationIds,
    CancellationToken cancellationToken = default)
  {
    List<Location> locations = await this._dbContext.Locations
      .Where(l => locationIds.Contains(l.Id))
      .ToListAsync(cancellationToken);

    return locations;
  }
}