namespace DirectoryService.Contracts.Dtos;

public record CreateDepartmentDto(string Name,
                                  string Slug,
                                  IEnumerable<Guid> locationIds,
                                  Guid? ParentId = null);