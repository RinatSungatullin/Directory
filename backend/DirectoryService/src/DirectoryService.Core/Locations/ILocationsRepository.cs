using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Locations;

public interface ILocationsRepository
{
  Task<Guid> AddAsync(Location location, CancellationToken cancellationToken = default);
  
  Task<IEnumerable<Location>> GetAllAsync(CancellationToken cancellationToken = default);
  
  Task<Location?> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default);
  
  Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default);
  
  Task<Guid?> GetByName(string name, CancellationToken cancellationToken = default);
  
  Task <IEnumerable<Location>> GetByIdListAsync(IEnumerable<Guid> locationIds, CancellationToken cancellationToken = default);
}