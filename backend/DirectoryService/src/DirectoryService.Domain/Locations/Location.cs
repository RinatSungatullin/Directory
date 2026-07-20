namespace DirectoryService.Domain.Locations;

public class Location
{
  public Guid Id { get; }
  
  public string? Name { get; private set; }
  
  public string? Address { get; private set; }
  
  public DateTime? CreatedAt { get; private set; }
  
  public DateTime? UpdatedAt { get; private set; }

  public Location(Guid id, string name, string address)
  {
    if (id == Guid.Empty)
      throw new InvalidDataException(nameof(id));
      
    if (string.IsNullOrEmpty(name))
      throw new InvalidDataException( nameof(name));
    
    if (string.IsNullOrEmpty(address))
      throw new InvalidDataException( nameof(address));

    
    this.Id = id;
    
    this.Name = name;
    
    this.Address = address;
    
    this.CreatedAt = DateTime.UtcNow;

    this.UpdatedAt = DateTime.UtcNow;
  }
}