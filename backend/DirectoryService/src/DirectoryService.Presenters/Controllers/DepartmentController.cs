using DirectoryService.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
  /// <summary>
  /// Создать отдел.
  /// </summary>
  /// <param name="departmentDto">Dto отдела.</param>
  /// <returns>Result.</returns>
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateDepartmentDto departmentDto)
  { 
    return Ok("department created");
  }

  /// <summary>
  /// Получить позицию по id.
  /// </summary>
  /// <param name="positionId">Id позиции.</param>
  /// <returns>Result.</returns>
  [HttpGet("{positionId:guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid positionId)
  {
    return Ok("department retrieved");
  }

  /// <summary>
  /// Полуичть все позиции.
  /// </summary>
  /// <returns>Result.</returns>
  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    return Ok("departments retrieved");
  }

  /// <summary>
  /// Обновить отдел.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <param name="departmentDto">Dto отдела.</param>
  /// <returns>Result.</returns>
  [HttpPut("{departmentId:guid}")]
  public async Task<IActionResult> Update([FromRoute] Guid departmentId, [FromBody] UpdateDepartmentDto departmentDto)
  {
    return Ok("department updated");
  }

  /// <summary>
  /// Удалить отдел.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <returns>Result.</returns>
  [HttpDelete("{departmentId:guid}")]
  public async Task<IActionResult> Delete([FromRoute] Guid departmentId)
  {
    return Ok("department deleted");
  }
}