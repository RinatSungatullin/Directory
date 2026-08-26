using System.Text.RegularExpressions;

namespace DirectoryService.Domain.Departments;

public class Department
{
  public Guid Id { get; private set; }
  
  public string? Name { get; private set; }
  
  public string? Slug { get; private set; }
  
  public string? Path { get;  private set; }
  
  public Guid? ParentId { get; private set; }
  
  public DateTime CreatedAt { get; private set; }
  
  public DateTime UpdatedAt { get; private set; }
  
  private readonly List<DepartmentLocation> _locations = [];
  
  private readonly List<DepartmentPosition> _positions = [];

  public IReadOnlyCollection<DepartmentLocation> Locations => _locations;

  public IReadOnlyCollection<DepartmentPosition> Positions => _positions;
  
  public Department(
    Guid id,
    string name,
    string slug,
    Guid? parentId,
    string? parentPath)
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

    this.Id = id; 

    this.Name = name;

    this.Slug = slug;
    
    if (parentId != null)
    {
      this.ParentId = parentId;

      this.Path = $"{parentPath}-{slug}";
    }

    else
    {
      this.Path = slug;
    }

    this.CreatedAt = DateTime.UtcNow;

    this.UpdatedAt = DateTime.UtcNow;
  }
  
  private Department()
  {
    
  }
}