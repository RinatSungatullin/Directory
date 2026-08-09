using DirectoryService.Domain.Addresses;

namespace DirectoryService.Domain.Locations;

public class Location
{
  public Guid Id { get; }

  public string Name { get; private set; } = null!;

  public Address Address { get; private set; } = null!;
  
  public DateTime? CreatedAt { get; private set; }
  
  public DateTime? UpdatedAt { get; private set; }

  public Location(Guid id, string name, Address address)
  {
    if (id == Guid.Empty)
      throw new InvalidDataException(nameof(id));
      
    if (string.IsNullOrEmpty(name))
      throw new InvalidDataException(nameof(name));
    
    this.Id = id;
    
    this.Name = name;
    
    this.Address = address;
    
    this.CreatedAt = DateTime.UtcNow;

    this.UpdatedAt = DateTime.UtcNow;
  }
  public Location()
  { }
}