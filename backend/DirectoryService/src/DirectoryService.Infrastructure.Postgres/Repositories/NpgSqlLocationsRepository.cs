using System.Data;
using Dapper;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Locations;
using DirectoryService.Infrastructure.Postgres.Database;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class NpgSqlLocationsRepository : ILocationsRepository
{
  private readonly NpgSqlConnectionFactory _connectionFactory;

  public NpgSqlLocationsRepository(NpgSqlConnectionFactory factory)
  {
    this._connectionFactory = factory;
  }
  
  public async Task<Guid> AddAsync(Location location)
  {
    using var connection = await this._connectionFactory.CreateConnection();

    const string locationInsertSql = """
                                        INSERT INTO locations (
                                        id, name, city, street, building,
                                        office_number, created_at, updated_at
                                        )
                                        VALUES (
                                        @Id, @Name, @City, @Street, @Building,
                                        @OfficeNumber, @CreatedAt, @UpdatedAt
                                        );
                                     """;
    
    await connection.ExecuteAsync(locationInsertSql, new
    {
      Id = location.Id,
      Name = location.Name,
      City = location.Address.City,
      Street = location.Address.Street,
      Building = location.Address.Building,
      OfficeNumber = location.Address.OfficeNumber,
      CreatedAt = location.CreatedAt,
      UpdatedAt = location.UpdatedAt
    });
    
    return location.Id;
  }

  public async Task<Location> GetByIdAsync(Guid locationId)
  {
    throw new DataException();
  }

  public async Task<Guid> UpdateAsync(Location location)
  {
    throw new DataException();
  }

  public async Task<Guid> DeleteAsync(Guid locationId)
  {
    throw new DataException();
  }

  public async Task<Guid> GetLocationByName(string name)
  {
    return Guid.Empty;
  }
}