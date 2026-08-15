using System.Data;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class EfCoreLocationsRepository : ILocationsRepository
{
  private readonly DirectoryServiceDbContext _dbContext;

  public EfCoreLocationsRepository(DirectoryServiceDbContext dbContext)
  {
    this._dbContext = dbContext;
  }
  
  public async Task<Guid> AddAsync(Location location)
  {
    await this._dbContext.AddAsync(location);
    
    await this._dbContext.SaveChangesAsync();
    
    return location.Id;
  }

  public Task<Location> GetByIdAsync(Guid locationId)
  {
    throw new DataException();
  }

  public Task<Guid> UpdateAsync(Location location)
  {
    throw new DataException();
  }

  public Task<Guid> DeleteAsync(Guid locationId)
  {
    throw new DataException();
  }

  public async Task<Guid> GetLocationByName(string name)
  {
    return Guid.Empty;
    
  }
}