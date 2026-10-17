using System.ComponentModel.DataAnnotations;
using System.Data;
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

  /// <summary>
  /// Добавить локацию.
  /// </summary>
  /// <param name="locationDto">Dto локации</param>
  /// <returns>Id локации.</returns>
  /// <exception cref="ValidationException">Ошибка валидации.</exception>
  /// <exception cref="InvalidDataException">Некорректные данные локации.</exception>
  public async Task<LocationDto> Add(CreateLocationDto locationDto)
  {
    var validationResult = await this._createLocationValidator.ValidateAsync(locationDto);

    if (!validationResult.IsValid)
    {
      throw new ValidationException(validationResult.Errors.ToString());
    }
    
    Guid? existsLocation = await this._locationsRepository.GetByName(locationDto.Name);

    if (existsLocation.HasValue)
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
    
    return new LocationDto(
      location.Id,
      location.Name,
      new AddressDto(
        location.Address.City,
        location.Address.Street,
        location.Address.Building,
        location.Address.OfficeNumber));
  }

  /// <summary>
  /// Получить все локации.
  /// </summary>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Коллекция локаций.</returns>
  public async Task<IEnumerable<Location>> GetAll(CancellationToken cancellationToken = default)
  {
    List<Location> locations = (await this._locationsRepository.GetAllAsync(cancellationToken)).ToList();
    
    return locations;
  }

  /// <summary>
  /// Получить локацию по id.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto локации.</returns>
  public async Task<LocationDto> GetById(Guid locationId, CancellationToken cancellationToken = default)
  {
    var location = await this._locationsRepository.GetByIdAsync(locationId, cancellationToken);

    return new LocationDto(
      location!.Id,
      location.Name,
      new AddressDto(
        location.Address.City,
        location.Address.Street,
        location.Address.Building,
        location.Address.OfficeNumber));
  }

  /// <summary>
  /// Удалить локацию.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id локации.</returns>
  public async Task<LocationDto> Delete(Guid locationId, CancellationToken cancellationToken = default)
  {
    var location = await this._locationsRepository.GetByIdAsync(locationId, cancellationToken);
    
    if (location == null)
      throw new DataException("Location not found");
    
    await this._locationsRepository.DeleteAsync(locationId, cancellationToken);
    
    return new LocationDto(
      location!.Id,
      location.Name,
      new AddressDto(
        location.Address.City,
        location.Address.Street,
        location.Address.Building,
        location.Address.OfficeNumber));
  }

  /// <summary>
  /// Обновить название локации.
  /// </summary>
  /// <param name="id">Id локации.</param>
  /// <param name="name">Новое название локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto локации.</returns>
  /// <exception cref="DataException">Локация не найдена.</exception>
  public async Task<LocationDto> UpdateName(Guid id, string name, CancellationToken cancellationToken = default)
  {
    var location = await this._locationsRepository.GetByIdAsync(id, cancellationToken);
    
    if (location == null)
      throw new DataException("Location not found");

    location.UpdateName(name);
    
    await this._locationsRepository.UpdateAsync(location, cancellationToken);
    
    return new LocationDto(
      location.Id,
      location.Name,
      new AddressDto(location.Address.City,
        location.Address.Street,
        location.Address.Building,
        location.Address.OfficeNumber));
  }

  /// <summary>
  /// Обновить адрес локации.
  /// </summary>
  /// <param name="addressDto">Dto адреса.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto локации.</returns>
  /// <exception cref="DataException">Локация не найдена.</exception>
  public async Task<LocationDto> UpdateAddress(
    UpdateAddressDto addressDto,
    CancellationToken cancellationToken = default)
  {
    var location = await this._locationsRepository.GetByIdAsync(addressDto.LocationId, cancellationToken);
    
    if (location == null)
      throw new DataException("Location not found");
    
    location.UpdateAddress(Address.Create(addressDto.City, addressDto.Street,
                                        addressDto.Building, addressDto.OfficeNumber));

    await this._locationsRepository.UpdateAsync(location, cancellationToken);
    
    return new LocationDto(
      location.Id,
      location.Name,
      new AddressDto(location.Address.City, location.Address.Street,
                      location.Address.Building, location.Address.OfficeNumber));
  }
}