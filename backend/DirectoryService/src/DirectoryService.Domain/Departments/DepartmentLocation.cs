namespace DirectoryService.Domain.Departments;

public class DepartmentLocation
{
  public Guid Id { get; private set; }
  
  public Guid DepartmentId { get; private set; }
  
  public Guid LocationId { get; private set; }
  
  public bool IsPrimary { get; private set; }

  public DepartmentLocation(Guid id, Guid departmentId, Guid locationId, bool isPrimary)
  {
    if (id == Guid.Empty)
      throw new InvalidDataException(nameof(id));
    
    if (departmentId == Guid.Empty)
      throw new InvalidDataException(nameof(departmentId));
    
    if (locationId == Guid.Empty)
      throw new InvalidDataException(nameof(locationId));
    
    this.Id = id;
    
    this.DepartmentId = departmentId;
    
    this.LocationId = locationId;
    
    this.IsPrimary = isPrimary;
  }
  
  private DepartmentLocation()
  { }
}