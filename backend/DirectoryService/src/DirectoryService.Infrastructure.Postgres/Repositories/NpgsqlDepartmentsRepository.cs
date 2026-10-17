using System.Data;
using Dapper;
using DirectoryService.Core.Departments;
using DirectoryService.Domain.Departments;
using DirectoryService.Infrastructure.Postgres.Database;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class NpgsqlDepartmentsRepository : IDepartmentsRepository
{
  private readonly IDbConnectionFactory _connectionFactory;

  public NpgsqlDepartmentsRepository(IDbConnectionFactory connectionFactory)
  {
    this._connectionFactory = connectionFactory;
  }
  
  /// <summary>
  /// Добавить отдел.
  /// </summary>
  /// <param name="department">Отдел.</param>
  /// <param name="departmentLocations">Локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id добавленного отдела.</returns>
  public async Task<Guid> AddAsync(
    Department department,
    IEnumerable<DepartmentLocation> departmentLocations,
    CancellationToken cancellationToken = default)
  {
    await using var connection = await this._connectionFactory.AddCreationAsync(cancellationToken);

    await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

    try
    {
      const string departmentInsertSql = """
                                         INSERT INTO departments (
                                             id,
                                             name,
                                             slug,
                                             path,
                                             parent_id,
                                             created_at,
                                             updated_at
                                         )
                                         VALUES (
                                             @Id,
                                             @Name,
                                             @Slug,
                                             @Path,
                                             @ParentId,
                                             @CreatedAt,
                                             @UpdatedAt
                                         );
                                         """;


      const string departmentLocationInsertSql = """
                                                 INSERT INTO department_locations (
                                                     id,
                                                     department_id,
                                                     location_id,
                                                     is_primary
                                                 )
                                                 VALUES (
                                                     @Id,
                                                     @DepartmentId,
                                                     @LocationId,
                                                     @IsPrimary
                                                 );
                                                 """;
      var departmentCommand = new CommandDefinition(
        departmentInsertSql,
        new
        {
          department.Id,
          department.Name,
          department.Slug,
          department.Path,
          department.ParentId,
          department.CreatedAt,
          department.UpdatedAt
        },
        transaction: transaction,
        cancellationToken: cancellationToken);
      
      var departmentLocationCommand = new CommandDefinition(
        departmentLocationInsertSql,
        departmentLocations,
        transaction: transaction,
        cancellationToken: cancellationToken);

      await connection.ExecuteAsync(departmentCommand);
      
      await connection.ExecuteAsync(departmentLocationCommand);
      
      await transaction.CommitAsync(cancellationToken);

      return department.Id;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      
      await transaction.RollbackAsync(cancellationToken);
      
      throw;
    }
  }

  /// <summary>
  /// Получить все подразделения.
  /// </summary>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Список подразделений.</returns>
  public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT
                           id AS Id,
                           name AS Name,
                           slug AS Slug,
                           path  AS Path,
                           parent_id AS ParentId,
                           created_at  AS CreatedAt,
                           updated_at AS UpdatedAt
                       FROM departments
                       """;
    
    var command = new CommandDefinition(
      sql,
      cancellationToken: cancellationToken);
    
    return await connection.QueryAsync<Department>(command);
  }

  /// <summary>
  /// Получить подразделение по id.
  /// </summary>
  /// <param name="departmentId">Id подразделения.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Подразделение.</returns>
  public async Task<Department?> GetByIdAsync(Guid? departmentId, CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       SELECT
                           id,
                           name,
                           slug,
                           path,
                           parent_id,
                           created_at,
                           updated_at
                       FROM departments
                       WHERE id = @Id
                       """;

    var command = new CommandDefinition(
      sql,
      new
      {
        Id = departmentId
      },
      cancellationToken: cancellationToken);
    
    return await connection.QuerySingleOrDefaultAsync<Department>(command);
  }

  /// <summary>
  /// Удалить подразделение.
  /// </summary>
  /// <param name="departmentId">Id подразделения.</param>
  /// <param name="cancellationToken"></param>
  /// <returns>Id удаленного подразделения.</returns>
  public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default)
  {
    using var connection = 
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       DELETE
                       FROM departments
                       WHERE id = @Id
                       """;

    var command = new CommandDefinition(
      sql,
      new
      {
        Id = departmentId
      },
      cancellationToken: cancellationToken);
    
    await connection.ExecuteAsync(command);
    
    return departmentId;
  }

  /// <summary>
  /// Получить связь локации с подразделением по id подразделения и id локации.
  /// </summary>
  /// <param name="departmentId">Id подразделения.</param>
  /// <param name="locationId">Id локации.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Связль подразделения и локации.</returns>
  public async Task<DepartmentLocation?> GetDepartmentLocationByDepartmentIdAndLocationIdAsync(
    Guid departmentId,
    Guid locationId,
    CancellationToken cancellationToken = default)
  {
    using var connection = 
      await _connectionFactory.AddCreationAsync(cancellationToken);
    
    const string sql = """
                     SELECT
                       id,
                       department_id,
                       location_id,
                       is_primary
                     FROM department_locations
                     WHERE department_id = @DepartmentId
                     AND location_id = @LocationId
                     """;
    var command = new CommandDefinition(
      sql,
      new
      {
        DepartmentId = departmentId,
        LocationId = locationId
      },
      cancellationToken: cancellationToken);
    
    return await connection.QuerySingleOrDefaultAsync<DepartmentLocation>(command);
  }

  /// <summary>
  /// Удалить связь подраделения с локацией.
  /// </summary>
  /// <param name="departmentLocationId">Id связи.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id связи.</returns>
  /// <exception cref="DataException"></exception>
  public async Task<Guid> DeleteDepartmentLocationAsync(
    Guid departmentLocationId,
    CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);
    
    const string sql = """
                       DELETE FROM department_locations
                       WHERE id = @DepartmentLocationId
                       """;
    
    var command = new CommandDefinition(
      sql,
      new
      {
        DepartmentLocationId = departmentLocationId
      },
      cancellationToken: cancellationToken);
    
    await connection.ExecuteAsync(command);

    return departmentLocationId;
  }

  /// <summary>
  /// Обновить подразделение.
  /// </summary>
  /// <param name="department">Подразделение.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id подразделения.</returns>
  public async Task<Guid> UpdateAsync(
    Department department,
    CancellationToken cancellationToken = default)
  {
    using var connection =
      await _connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       UPDATE departments
                       SET name = @DepartmentName,
                           slug = @DepartmentSlug
                       WHERE id = @DepartmentId
                       """;
    
    var command = new CommandDefinition(
      sql,
      new
      {
        DepartmentName = department.Name,
        DepartmentSlug = department.Slug,
        DepartmentId = department.Id
      },
      cancellationToken: cancellationToken);
    
    await connection.ExecuteAsync(command);
    
    return department.Id;
  }

  /// <summary>
  /// Добавить связь локации и подразделения.
  /// </summary>
  /// <param name="departmentLocation">Связь локации и подразделения.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Id связи.</returns>
  public async Task<Guid> AddDepartmentLocationAsync(
    DepartmentLocation departmentLocation,
    CancellationToken cancellationToken = default)
  {
    await using var connection =
      await this._connectionFactory.AddCreationAsync(cancellationToken);

    const string sql = """
                       INSERT INTO department_locations
                       (id,
                        department_id,
                        location_id,
                        is_primary)
                        VALUES
                        (@Id,
                         @DepartmentId,
                         @LocationId,
                         @IsPrimary)
                       """;

    var command = new CommandDefinition(
      sql,
      new
      {
        departmentLocation.Id,
        departmentLocation.DepartmentId,
        departmentLocation.LocationId,
        departmentLocation.IsPrimary
      },
      cancellationToken: cancellationToken);
    
    await connection.ExecuteAsync(command);
    
    return departmentLocation.Id;
  }
}