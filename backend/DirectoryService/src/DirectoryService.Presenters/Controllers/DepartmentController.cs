using DirectoryService.Contracts.Dtos;
using DirectoryService.Core.Departments;
using DirectoryService.Domain.Departments;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presenters.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
  private readonly DepartmentsService _departmentsService;

  public DepartmentController(DepartmentsService departmentsService)
  {
    this._departmentsService = departmentsService;
  }
  
  /// <summary>
  /// Создать отдел.
  /// </summary>
  /// <param name="departmentDto">Dto отдела.</param>
  /// <returns>Result.</returns>
  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateDepartmentDto departmentDto)
  {
    var department = await this._departmentsService.Add(departmentDto);
    
    return Ok(department);
  }

  /// <summary>
  /// Получить отдел по id.
  /// </summary>
  /// <param name="departmentId">Id позиции.</param>
  /// <returns>Result.</returns>
  [HttpGet("{departmentId:guid}")]
  public async Task<IActionResult> GetById([FromRoute] Guid departmentId)
  {
    var department = await this._departmentsService.GetById(departmentId);
    
    return Ok(department);
  }

  /// <summary>
  /// Получить все отделы.
  /// </summary>
  /// <returns>Список отделов.</returns>
  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var departments = (await this._departmentsService.GetAll()).ToList();
    
    return Ok(departments);
  }

  /// <summary>
  /// Удалить отдел.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <returns>Result.</returns>
  [HttpDelete("{departmentId:guid}")]
  public async Task<IActionResult> Delete([FromRoute] Guid departmentId)
  {
    var department = await this._departmentsService.Delete(departmentId);
    
    return Ok(department);
  }

  /// <summary>
  /// Удалить связь отдела с локацией.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <param name="locationId"></param>
  /// <returns>Result.</returns>
  [HttpDelete("departments/{departmentId:guid}/locations/{locationId:guid}")]
  public async Task<IActionResult> DeleteDepartmentLocationAsync(
    [FromRoute] Guid departmentId,
    [FromRoute] Guid locationId)
  {
    var departmentLocation = await this._departmentsService
      .DeleteDepartmentLocation(departmentId, locationId);

    return Ok(departmentLocation);
  }

  /// <summary>
  /// Обновить название.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <param name="name">Новое название.</param>
  /// <returns>Dto отдела.</returns>
  [HttpPatch("/departments/update-name/{departmentId:guid}/{name}")]
  public async Task<IActionResult> UpdateName(
    [FromRoute] Guid departmentId,
    [FromRoute] string name)
  {
    var department = await this._departmentsService.UpdateName(departmentId, name);
    
    return Ok(department);
  }
  
  /// <summary>
  /// Обновить slug.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <param name="slug">Slug.</param>
  /// <returns>Dto отдела.</returns>
  [HttpPatch("/departments/update-slug/{departmentId:guid}/{slug}")]
  public async Task<IActionResult> UpdateSlug([FromRoute] Guid departmentId, [FromRoute] string slug)
  {
    var department = await this._departmentsService.UpdateSlug(departmentId, slug);
    
    return Ok(department);
  }

  /// <summary>
  /// Добавить связь локации с подразделением.
  /// </summary>
  /// <param name="departmentId">Id подразделения.</param>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Dto связи.</returns>
  [HttpPost("departments/{departmentId}/locations/{locationId}")]
  public async Task<IActionResult> AddDepartmentLocationAsync(
    [FromRoute] Guid departmentId,
    [FromRoute] Guid locationId)
  {
    var departmentLocationId = await this._departmentsService
      .AddDepartmentLocation(departmentId, locationId);
    
    return Ok(departmentLocationId);
  }
}