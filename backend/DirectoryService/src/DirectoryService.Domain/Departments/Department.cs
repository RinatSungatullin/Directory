using System.Text.RegularExpressions;

namespace DirectoryService.Domain.Departments;

public class Department
{
  public Guid? Id { get; }
  
  public string? Name { get; private set; }
  
  public string? Slug { get; private set; }
  
  public string? Path { get;  private set; }
  
  public Guid? ParentId { get; private set; }
  
  public DateTime? CreatedAt { get; private set; }
  
  public DateTime? UpdatedAt { get; private set; }
  
  private readonly List<DepartmentLocation> _locations;
  
  private readonly List<DepartmentPosition> _positions;

  public IReadOnlyCollection<DepartmentLocation> Locations => _locations;

  public IReadOnlyCollection<DepartmentPosition> Positions => _positions;
  
  public Department(Guid id, string name, string slug, string path, Guid? parentId,
                    IEnumerable<DepartmentPosition> positions, IEnumerable<DepartmentLocation> locations )
  { 
    if (string.IsNullOrEmpty(name))
      throw new InvalidDataException( nameof(name));
    
    if (string.IsNullOrEmpty(slug) ||
      !Regex.IsMatch(
          slug,
          @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
          RegexOptions.None,
          TimeSpan.FromMilliseconds(100)))
    {
      throw new ArgumentException("Invalid slug", nameof(slug));
    }
    
    if (string.IsNullOrEmpty(path) ||
        !path.EndsWith($"/{slug}", StringComparison.Ordinal))
      throw new InvalidDataException( nameof(path));
    
    if (parentId.HasValue && parentId.Value == Guid.Empty)
      throw new InvalidDataException(nameof(parentId));
    
    this.Id = id;
    
    this.Name = name;
    
    this.Slug = slug;
    
    this.Path = path;
    
    this.ParentId = parentId;

    this.CreatedAt = DateTime.UtcNow;
    
    this.UpdatedAt = DateTime.UtcNow;
    
    this._positions = positions.ToList();
    
    this._locations = locations.ToList();
  }
}