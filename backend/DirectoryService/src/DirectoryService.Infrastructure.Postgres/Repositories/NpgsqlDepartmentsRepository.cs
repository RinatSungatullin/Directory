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
                                                     location_id
                                                 )
                                                 VALUES (
                                                     @Id,
                                                     @DepartmentId,
                                                     @LocationId
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

  public Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

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
                       FROM locations
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

  public Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }
}