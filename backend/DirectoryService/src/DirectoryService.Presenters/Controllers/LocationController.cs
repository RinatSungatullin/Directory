using DirectoryService.Contracts.Dtos;
using DirectoryService.Core;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
  private readonly LocationService _locationService;
  
  public LocationController(LocationService locationService)
  {
    this._locationService = locationService;
  }
  
  /// <summary>
  /// Создать локацию.
  /// </summary>
  /// <param name="locationDto">Dto локации.</param>
  /// <returns>Result.</returns>
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateLocationDto locationDto)
  {
    var location = await this._locationService.Add(locationDto);
    
    return Ok(location);
  }

  /// <summary>
  /// Получить локацию по id.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Result.</returns>
  [HttpGet("{locationId:guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid locationId)
  {
    var location = await this._locationService.GetById(locationId);

    return Ok(location);
  }

  /// <summary>
  /// Получить все локации.
  /// </summary>
  /// <returns>Result.</returns>
  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var locations = await this._locationService.GetAll();
    
    return Ok(locations);
  }

  /// <summary>
  /// Удалить локацию.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Result.</returns>
  [HttpDelete("{locationId:guid}")]
  public async Task<IActionResult> Delete([FromRoute] Guid locationId)
  {
    var location = await this._locationService.Delete(locationId);
    
    return Ok(location);
  }

  /// <summary>
  /// Обновить название.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <param name="name">Новое наименование.</param>
  /// <returns>Dto локации.</returns>
  [HttpPatch("locations/update-name/{locationId:guid}/{name}")]
  public async Task<IActionResult> UpdateName([FromRoute] Guid locationId, [FromRoute] string name)
  {
    var location = await this._locationService.UpdateName(locationId, name)!;
    
    return Ok(location);
  }

  /// <summary>
  /// Обновить адрес локации.
  /// </summary>
  /// <param name="addressDto">Dto адреса.</param>
  /// <returns>Dto обновленной локации.</returns>
  [HttpPatch("locations/update-address/")]
  public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto addressDto)
  {
    var location = await this._locationService.UpdateAddress(addressDto);
    
    return Ok(location);
  }
}