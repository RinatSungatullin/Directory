using System.Data;
using Dapper;
using DirectoryService.Core.Locations;
using DirectoryService.Domain.Addresses;
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
  
  /// <summary>
  /// Добавить локацию.
  /// </summary>
  /// <param name="location">Локация.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id добавленной локации.</returns>
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

  /// <summary>
  ///  Получить все локации.
  /// </summary>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Список локаций.</returns>
  public async Task<IEnumerable<Location>> GetAllAsync(
    CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT
                           id,
                           name,
                           created_at AS CreatedAt,
                           updated_at AS UpdatedAt,
                           
                           city AS City,
                           street AS Street,
                           building AS Building,
                           office_number AS OfficeNumber
                           
                       FROM locations
                       """;

    var command = new CommandDefinition(
      sql,
      cancellationToken: cancellationToken);

    return await connection.QueryAsync<Location, Address, Location>(
      command,
      (location, address) =>
      {
        location.UpdateAddress(address);

        return location;
      },
      splitOn: "city");
  }

  /// <summary>
  /// Получить локацию по id.
  /// </summary>
  /// <param name="locationId">Id локациию.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Локация.</returns>
  public async Task<Location?> GetByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT
                           id,
                           name,
                           created_at AS CreatedAt,
                           updated_at AS UpdatedAt,
                           city AS City,
                           street AS Street,
                           building AS Building,
                           office_number AS OfficeNumber
                           
                       FROM locations
                       WHERE id = @Id
                       """;

    var command = new CommandDefinition(
      sql,
      new
      {
        Id = locationId
      },
      cancellationToken: cancellationToken);

    var locations = await connection.QueryAsync<Location, Address, Location>(
      command,
      (location, address) =>
      {
        location.UpdateAddress(address);
        return location;
      },
      splitOn: "City");

    return locations.FirstOrDefault();
  }

  /// <summary>
  /// Удалить локацию.
  /// </summary>
  /// <param name="locationId">Id локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id локации.</returns>
  public async Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken = default)
  {
    using var connection = 
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       DELETE
                       FROM locations
                       WHERE id = @Id
                       """;

    var command = new CommandDefinition(
      sql,
      new
      {
        Id = locationId
      },
      cancellationToken: cancellationToken);
    
    await connection.ExecuteAsync(command);
    
    return locationId;
  }

  /// <summary>
  /// Получить лакацию по наименованию.
  /// </summary>
  /// <param name="name">Имя локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Локация.</returns>
  public async Task<Guid?> GetByName(
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

  /// <summary>
  /// Получить коллекцию локаций по коллекции id.
  /// </summary>
  /// <param name="locationIds">Коллекция id.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Локации.</returns>
  public async Task<IEnumerable<Location>> GetByIdListAsync(IEnumerable<Guid> locationIds, CancellationToken cancellationToken = default)
  {
    using var connection = await _connectionFactory.AddCreationAsync(cancellationToken);
    
    const string sql = """
                       SELECT id, name
                       FROM locations
                       WHERE id = ANY(@LocationIds);
                       """;
    
    return await connection.QueryAsync<Location>(sql, new {LocationIds = locationIds});
  }

  /// <summary>
  /// Обновить локацию.
  /// </summary>
  /// <param name="location">Локация.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id локации.</returns>
  public async Task<Guid> UpdateAsync(Location location, CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);
    
    const string sql = """
                       UPDATE locations
                       SET name = @LocationName,
                           city = @LocationCity,
                           street = @LocationStreet,
                           building = @LocationBuilding,
                           office_number = @LocationOfficeNumber
                       WHERE id = @LocationId
                       """;
    
    var command = new CommandDefinition(
      sql,
      new
      {
        LocationName = location.Name,
        LocationCity = location.Address.City,
        LocationStreet = location.Address.Street,
        LocationBuilding = location.Address.Building,
        LocationOfficeNumber = location.Address.OfficeNumber,
        LocationId = location.Id
      },
      cancellationToken: cancellationToken);
      
    await connection.ExecuteAsync(command);
    
    return location.Id;
  }
}