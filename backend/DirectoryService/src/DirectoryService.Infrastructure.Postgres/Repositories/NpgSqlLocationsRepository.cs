using System.Data;
using Dapper;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Locations;
using DirectoryService.Infrastructure.Postgres.Database;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class NpgSqlLocationsRepository : ILocationsRepository
{
  private readonly IDbConnectionFactory _connectionFactory;

  public NpgSqlLocationsRepository(IDbConnectionFactory factory)
  {
    this._connectionFactory = factory;
  }
  
  public async Task<Guid> AddAsync(Location location, CancellationToken cancellationToken = default)
  {
    using var connection = await this._connectionFactory.AddCreationAsync(cancellationToken);

    
    
    const string locationInsertSql = """
                                     INSERT INTO locations (
                                         id,
                                         name,
                                         city,
                                         street,
                                         building,
                                         office_number,
                                         created_at,
                                         updated_at
                                     )
                                     VALUES (
                                         @Id,
                                         @Name,
                                         @City,
                                         @Street,
                                         @Building,
                                         @OfficeNumber,
                                         @CreatedAt,
                                         @UpdatedAt
                                     );
                                     """;

    var command = new CommandDefinition(
      locationInsertSql,
      new
      {
        location.Id,
        location.Name,
        City = location.Address.City,
        Street = location.Address.Street,
        Building = location.Address.Building,
        OfficeNumber = location.Address.OfficeNumber,
        location.CreatedAt,
        location.UpdatedAt
      },
      cancellationToken: cancellationToken);

    await connection.ExecuteAsync(command);

    return location.Id;
  }

  public async Task<IEnumerable<Location>> GetAllAsync(
    CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT
                           id,
                           name,
                           city,
                           street,
                           building,
                           office_number,
                           created_at,
                           updated_at
                       FROM locations
                       """;

    var command = new CommandDefinition(
      sql,
      cancellationToken: cancellationToken);

    return await connection.QueryAsync<Location>(command);
  }

  public async Task<Location?> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Guid> UpdateAsync(Guid locationId, Location newLocation, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Guid?> GetLocationByName(
    string name,
    CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT id
                       FROM locations
                       WHERE name = @Name
                       """;

    var command = new CommandDefinition(
      sql,
      new { Name = name },
      cancellationToken: cancellationToken);

    return await connection.QuerySingleOrDefaultAsync<Guid?>(command);
  }
}