namespace DirectoryService.Contracts.Dtos;

public record UpdateDepartmentDto(string Name, string Slug, Guid? ParentId);