using System.Data;
using System.Data.Common;

namespace DirectoryService.Infrastructure.Postgres.Database;

public interface IDbConnectionFactory
{
  Task<DbConnection> AddCreationAsync(CancellationToken cancellationToken = default);
}