namespace DirectoryService.Domain;

public class DepartmentPosition
{
  public Guid Id { get; }
  
  public Guid DepartmentId { get; private set; }
  
  public Guid PositionId { get; private set; }

  public DepartmentPosition(Guid id, Guid departmentId, Guid positionId)
  {
    if (id == Guid.Empty)
      throw new InvalidDataException(nameof(id));
    
    if (departmentId == Guid.Empty)
      throw new InvalidDataException(nameof(departmentId));
    
    if (positionId == Guid.Empty)
      throw new InvalidDataException(nameof(positionId));
    
    this.Id = id;
    
    this.DepartmentId = departmentId;
    
    this.PositionId = positionId;
  }
}