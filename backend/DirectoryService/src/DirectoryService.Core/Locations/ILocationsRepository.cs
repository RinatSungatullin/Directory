using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Locations;

public interface ILocationsRepository
{
  Task<Guid> AddAsync(Location location, CancellationToken cancellationToken = default);
  
  Task<Location> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default);
  
  Task<Guid> UpdateAsync(Location location, CancellationToken cancellationToken = default);
  
  Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default);
  
  Task<Guid?> GetLocationByName(string name, CancellationToken cancellationToken = default);
}