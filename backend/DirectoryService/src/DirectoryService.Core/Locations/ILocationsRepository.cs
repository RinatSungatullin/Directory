using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Locations;

public interface ILocationsRepository
{
  Task<Guid> AddAsync(Location location);
  
  Task<Location> GetByIdAsync(Guid locationId);
  
  Task<Guid> UpdateAsync(Location location);
  
  Task<Guid> DeleteAsync(Guid locationId);
  
  Task<Guid?> GetLocationByName(string name);
}