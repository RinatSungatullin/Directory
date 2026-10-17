using System.Data;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Locations;

namespace DirectoryService.Core.Departments;

public class DepartmentsService
{
  private readonly IDepartmentsRepository _departmentRepository;
  
  private readonly ILocationsRepository _locationsRepository;
  
  private readonly CreateDepartmentValidator _createDepartmentValidator;
  
  public DepartmentsService(
    IDepartmentsRepository departmentRepository,
    ILocationsRepository locationsRepository,
    CreateDepartmentValidator createDepartmentValidator)
  {
    this._departmentRepository = departmentRepository;

    this._locationsRepository = locationsRepository;
    
    this._createDepartmentValidator = createDepartmentValidator;
  }
  
  /// <summary>
  /// Добавить отдел.
  /// </summary>
  /// <param name="departmentDto">Dto отдела.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto Отдела.</returns>
  /// <exception cref="DataException">Некорректные поля.</exception>
  public async Task<DepartmentDto> Add(CreateDepartmentDto departmentDto, CancellationToken cancellationToken = default)
  {
    var validationResult = await this._createDepartmentValidator.ValidateAsync(departmentDto, cancellationToken);

    if (!validationResult.IsValid)
    {
      throw new DataException(string.Join("; ", validationResult.Errors.Select(e=>
        e.ErrorMessage)));
    }
    
    List<Location> locations = (await this._locationsRepository
      .GetByIdListAsync(departmentDto.locationIds, cancellationToken)).ToList();
    
    bool isValidLocations =
      departmentDto.locationIds.All(id
        => locations.Any(l => l.Id == id));

    if (!isValidLocations)
    {
      throw new DataException("locations not found");
    }

    Department? parentDepartment = null;
    
    if (departmentDto.ParentId != null)
    {
      parentDepartment = await this._departmentRepository.GetByIdAsync(
        departmentDto.ParentId,
        cancellationToken);

      if (parentDepartment == null)
      {
        throw new DataException("parent department not found");
      }
    }
    
    Guid departmentId = Guid.NewGuid();
    
    Department department = new Department(departmentId,
                                            departmentDto.Name,
                                            departmentDto.Slug,
                                            parentDepartment?.Id,
                                            parentDepartment?.Path);

    var departmentLocations = departmentDto.locationIds
      .Select(locationId => new DepartmentLocation(
        Guid.NewGuid(),
        departmentId,
        locationId,
        false))
      .ToList();
    
    await this._departmentRepository.AddAsync(department, departmentLocations, cancellationToken);
    
    return new DepartmentDto(department.Id, department.Name!, department.Slug!, department.Path!);
  }

  /// <summary>
  /// Получить отдел по id.
  /// </summary>
  /// <param name="departmentId">Id.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns></returns>
  public async Task<DepartmentDto> GetById(
    Guid departmentId,
    CancellationToken cancellationToken = default)
  {
    var department = (await this._departmentRepository.GetByIdAsync(departmentId, cancellationToken))!;
    
    return new DepartmentDto(department.Id, department.Name!, department.Slug!, department.Path!);
  }

  /// <summary>
  /// Получить все отделы.
  /// </summary>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Список отделов.</returns>
  public async Task<IEnumerable<DepartmentDto>> GetAll(CancellationToken cancellationToken = default)
  {
    var departments = (await this._departmentRepository
      .GetAllAsync(cancellationToken));

    var departmentsDto = departments.Select(d =>
      new DepartmentDto(d.Id, d.Name!, d.Slug!, d.Path!));
    
    return departmentsDto;
  }

  public async Task<DepartmentDto> Delete(Guid departmentId, CancellationToken cancellationToken = default)
  {
    var department = await this._departmentRepository.GetByIdAsync(departmentId, cancellationToken);
    
    if (department == null)
      throw new DataException("department not found");
    
    await this._departmentRepository.DeleteAsync(departmentId, cancellationToken);
    
    return new DepartmentDto(department.Id, department.Name!, department.Slug!, department.Path!);
  }

  /// <summary>
  /// Обновить имя.
  /// </summary>
  /// <param name="departmentId">Id.</param>
  /// <param name="name">Новое название.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto отдела.</returns>
  /// <exception cref="DataException">Некорректный Id.</exception>
  public async Task<DepartmentDto> UpdateName(
    Guid departmentId,
    string name,
    CancellationToken cancellationToken = default)
  {
    var department = (await this._departmentRepository.GetByIdAsync(departmentId, cancellationToken))!;
    
    if (department == null)
      throw new DataException("department not found");
    
    department.UpdateName(name);

    await this._departmentRepository.UpdateAsync(department, cancellationToken);
    
    return new DepartmentDto(department.Id, department.Name!, department.Slug!, department.Path!);
  }

  /// <summary>
  /// Обновить slug.
  /// </summary>
  /// <param name="departmentId">Id отдела.</param>
  /// <param name="slug">Slug.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Dto отдела.</returns>
  /// <exception cref="DataException">Отдел не найден.</exception>
  public async Task<DepartmentDto> UpdateSlug(
    Guid departmentId,
    string slug,
    CancellationToken cancellationToken = default)
  {
    var department = (await this._departmentRepository.GetByIdAsync(departmentId, cancellationToken))!;
    
    if (department == null)
      throw new DataException("department not found");
    
    department.UpdateSlug(slug);

    await this._departmentRepository.UpdateAsync(department, cancellationToken);
    
    return new DepartmentDto(department.Id, department.Name!, department.Slug!, department.Path!);
  }

  /// <summary>
  /// Добавить локацию к подразделению.
  /// </summary>
  /// <param name="departmentId">Id подразделения.</param>
  /// <param name="locationId">Id локации.</param>
  /// <returns>Id связи локации с подразделением.</returns>
  public async Task<Guid> AddDepartmentLocation(Guid departmentId, Guid locationId)
  {
    var departmentLocationQuery = await this._departmentRepository
      .GetDepartmentLocationByDepartmentIdAndLocationIdAsync(departmentId, locationId);
    
    if (departmentLocationQuery != null)
      throw new DataException("departmentLocation already exists");
    
    var location = await this._locationsRepository.GetByIdAsync(locationId);
    
    if (location == null)
      throw new DataException("location not found");
    
    var department = await this._departmentRepository.GetByIdAsync(departmentId);
    
    if (department == null)
      throw new DataException("department not found");
      
    var departmentLocation = new DepartmentLocation(
      Guid.NewGuid(),
      departmentId,
      locationId,
      false);
    
    await this._departmentRepository.AddDepartmentLocationAsync(departmentLocation);
    
    return departmentLocation.Id;
  }

  public async Task<Guid> DeleteDepartmentLocation(
    Guid departmentId,
    Guid locationId,
    CancellationToken cancellationToken = default)
  {
    var departmentLocation = await this._departmentRepository.GetDepartmentLocationByDepartmentIdAndLocationIdAsync(
      departmentId, locationId, cancellationToken);

    if (departmentLocation == null)
      throw new DataException("department location not found");
    
    await this._departmentRepository.DeleteDepartmentLocationAsync(departmentLocation.Id, cancellationToken);
    
    return departmentId;
  }
}