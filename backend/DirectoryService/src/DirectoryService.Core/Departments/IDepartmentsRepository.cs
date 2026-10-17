using DirectoryService.Domain.Departments;

namespace DirectoryService.Core.Departments;

public interface IDepartmentsRepository
{
  Task<Guid> AddAsync(Department department, IEnumerable<DepartmentLocation> departmentLocations,
                        CancellationToken cancellationToken = default);
  
  Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
  
  Task<Department?> GetByIdAsync(Guid? departmentId, CancellationToken cancellationToken = default);
  
  Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default);
  
  Task<DepartmentLocation?> GetDepartmentLocationByDepartmentIdAndLocationIdAsync(
    Guid departmentId, Guid locationId, CancellationToken cancellationToken = default);
  
  Task<Guid> DeleteDepartmentLocationAsync(
    Guid departmentLocationId,
    CancellationToken cancellationToken = default);

  Task<Guid> UpdateAsync(Department department, CancellationToken cancellationToken = default);

  Task<Guid> AddDepartmentLocationAsync(
    DepartmentLocation departmentLocation,
    CancellationToken cancellationToken = default);
}