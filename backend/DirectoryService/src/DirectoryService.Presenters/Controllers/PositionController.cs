using DirectoryService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionController : ControllerBase
{
  /// <summary>
  /// Создать позицию.
  /// </summary>
  /// <param name="positionDto">Dto позиции.</param>
  /// <returns>Result.</returns>
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreatePositionDto positionDto)
  {
    return Ok("position created");
  }

  /// <summary>
  /// Получить позицию по id.
  /// </summary>
  /// <param name="positionId">Id позиции.</param>
  /// <returns>Result.</returns>
  [HttpGet("{positionId:guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid positionId)
  {
    return Ok("position retrieved");
  }
  
  /// <summary>
  /// Получить все позиции.
  /// </summary>
  /// <returns>Result.</returns>
  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    return Ok("positions retrieved");
  }
  
  /// <summary>
  /// Обновить позицию.
  /// </summary>
  /// <param name="positionId">Id позиции.</param>
  /// <param name="positionDto">Dto с новыми полями.</param>
  /// <returns>Result.</returns>
  [HttpPut("{positionId:guid}")]
  public async Task<IActionResult> Update([FromRoute] Guid positionId, [FromBody] UpdatePositionDto positionDto)
  {
    return Ok("position updated");
  }

  /// <summary>
  /// Удалить позицию.
  /// </summary>
  /// <param name="positionId">Id позиции.</param>
  /// <returns>Result.</returns>
  [HttpDelete("{positionId:guid}")]
  public async Task<IActionResult> Delete([FromRoute] Guid positionId)
  {
    return Ok("position deleted");
  }
}