using DirectoryService.Contracts.Dtos;
using DirectoryService.Core;
using DirectoryService.Core.Locations;
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
    Guid locationId = await this._locationService.SaveAsync(locationDto);
    
    return Ok(locationId);
  }

  /// <summary>
  /// Получить локацию по id.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Result.</returns>
  [HttpGet("{locationId:guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid locationId)
  {
    return Ok("location retrieved");
  }

  /// <summary>
  /// Получить все локации.
  /// </summary>
  /// <returns>Result.</returns>
  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    return Ok("locations retrieved");
  }
  
  /// <summary>
  /// Обновить локацию.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <param name="locationDto">Dto локации.</param>
  /// <returns>Result.</returns>
  [HttpPut("{locationId:guid}")]
  public async Task<IActionResult> Update([FromRoute] Guid locationId, [FromBody] UpdateLocationDto locationDto)
  {
    return Ok("location updated");
  }

  /// <summary>
  /// Удалить локацию.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Result.</returns>
  [HttpDelete("{locationId:guid}")]
  public async Task<IActionResult> Delete([FromRoute] Guid locationId)
  {
    return Ok("location deleted");
  }
}