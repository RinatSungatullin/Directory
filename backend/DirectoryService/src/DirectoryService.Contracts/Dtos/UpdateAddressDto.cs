namespace DirectoryService.Contracts.Dtos;

public record UpdateAddressDto(Guid LocationId, string City, string Street, string Building, string OfficeNumber);