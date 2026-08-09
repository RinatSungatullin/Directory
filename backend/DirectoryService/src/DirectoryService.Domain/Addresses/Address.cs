using System.Text.RegularExpressions;

namespace DirectoryService.Domain.Addresses;

public record Address(string City, string Street, string Building, string OfficeNumber)
{
  public static Address Create(string city, String street, String building, String officeNumber)
  {
    if (string.IsNullOrWhiteSpace(city))
    {
      throw new ArgumentException("Invalid city", nameof(city));
    }
    
    if (string.IsNullOrWhiteSpace(street))
    {
      throw new ArgumentException("Invalid city", nameof(street));
    }
    
    if (string.IsNullOrWhiteSpace(building))
    {
      throw new ArgumentException("Invalid city", nameof(building));
    }
    
    if (string.IsNullOrWhiteSpace(officeNumber))
    {
      throw new ArgumentException("Invalid city", nameof(officeNumber));
    }
    
    return new Address(city, street, building, officeNumber);
  }
}