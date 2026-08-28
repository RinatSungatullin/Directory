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
  
  public async Task<DepartmentDto> Add(CreateDepartmentDto departmentDto, CancellationToken cancellationToken = default)
  {
    var validationResult = await this._createDepartmentValidator.ValidateAsync(departmentDto, cancellationToken);

    if (!validationResult.IsValid)
    {
      throw new DataException(validationResult.Errors.ToString());
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
    
    return  new DepartmentDto(department.Id, department.Name, department.Slug, department.Path);
  }

  public async Task<Department?> GetById(Guid departmentId, CancellationToken cancellationToken = default)
  {
    Department? department = await this._departmentRepository.GetByIdAsync(departmentId, cancellationToken);
    
    return department;
  }
}