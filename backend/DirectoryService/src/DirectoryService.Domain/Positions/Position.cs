namespace DirectoryService.Domain.Positions;

public class Position
{
  public Guid Id { get; }
  
  public string? Name { get; private set; }
  
  public DateTime? CreatedAt { get; private set; }
  
  public DateTime? UpdatedAt { get; private set; }

  public Position(Guid id, string name)
  {
    this.Id = id;
    
    this.Name = name;
    
    this.CreatedAt = DateTime.UtcNow;
    
    this.UpdatedAt = DateTime.UtcNow;
  }
}