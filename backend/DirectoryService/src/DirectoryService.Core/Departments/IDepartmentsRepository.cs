using DirectoryService.Domain.Departments;

namespace DirectoryService.Core.Departments;

public interface IDepartmentsRepository
{
  Task<Guid> AddAsync(Department department, IEnumerable<DepartmentLocation> departmentLocations,
                        CancellationToken cancellationToken = default);
  
  Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
  
  Task<Department?> GetByIdAsync(Guid? departmentId, CancellationToken cancellationToken = default);
  
  Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default);
}