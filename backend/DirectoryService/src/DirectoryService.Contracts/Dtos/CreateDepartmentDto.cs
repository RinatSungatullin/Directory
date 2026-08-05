namespace DirectoryService.Contracts.Dtos;

public record CreateDepartmentDto(string Name, string Slug, Guid? ParentId);