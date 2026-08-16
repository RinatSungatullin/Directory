using System.Data;

namespace DirectoryService.Infrastructure.Postgres.Database;

public interface IDbConnectionFactory
{
  Task<IDbConnection> AddCreationAsync(CancellationToken cancellationToken = default);
}