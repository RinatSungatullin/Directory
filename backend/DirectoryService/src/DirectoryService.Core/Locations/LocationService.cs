using System.ComponentModel.DataAnnotations;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.Addresses;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Locations;

public class LocationService
{
  private readonly ILocationsRepository _locationsRepository;
  
  private readonly CreateLocationValidator _createLocationValidator;
  
  public LocationService(ILocationsRepository locationsRepository,
                          CreateLocationValidator createLocationValidator)
  {
    this._locationsRepository = locationsRepository;
    
    this._createLocationValidator = createLocationValidator;
  }

  public async Task<Guid> SaveAsync(CreateLocationDto locationDto)
  {
    var validationResult = await this._createLocationValidator.ValidateAsync(locationDto);

    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors.ToString());
    }
    
    Guid existsLocation = await this._locationsRepository.GetLocationByName(locationDto.Name);

    if (existsLocation != Guid.Empty)
    {
      throw new InvalidDataException("Имя уже существует");
    }
    
    Guid newLocationId = Guid.NewGuid();

    Address address = Address.Create(locationDto.Address.City,
                                      locationDto.Address.Street,
                                      locationDto.Address.Building,
                                      locationDto.Address.OfficeNumber);
    
    Location location = new Location(newLocationId, locationDto.Name, address);
    
    await this._locationsRepository.AddAsync(location);
    
    return newLocationId;
  }
}