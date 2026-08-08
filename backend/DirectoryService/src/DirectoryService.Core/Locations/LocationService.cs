using System.ComponentModel.DataAnnotations;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Locations;

public class LocationService
{
  private readonly ILocationRepository _locationRepository;
  
  private readonly CreateLocationValidator _createLocationValidator;
  
  public LocationService(ILocationRepository locationRepository,
                          CreateLocationValidator createLocationValidator)
  {
    this._locationRepository = locationRepository;
    
    this._createLocationValidator = createLocationValidator;
  }

  public async Task<Guid> SaveAsync(CreateLocationDto locationDto)
  {
    var validationResult = await this._createLocationValidator.ValidateAsync(locationDto);

    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors.ToString());
    }
    
    Guid existsLocation = await this._locationRepository.GetLocationByName(locationDto.Name);

    if (existsLocation == Guid.Empty)
    {
      throw new InvalidDataException("Имя уже существует");
    }
    
    Guid newLocationId = Guid.NewGuid();
    
    Location location = new Location(newLocationId, locationDto.Name, locationDto.Address);
    
    await this._locationRepository.AddAsync(location);
    
    return newLocationId;
  }
}