using System.Data;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Postgres.Database;

public class NpgSqlConnectionFactory : IDisposable, IAsyncDisposable, IDbConnectionFactory
{
  private readonly NpgsqlDataSource _dataSource;
  private bool _disposed;

  public NpgSqlConnectionFactory(IConfiguration configuration, ILoggerFactory loggerFactory)
  {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(
      configuration.GetConnectionString("DefaultConnection"))
      .UseLoggerFactory(loggerFactory);
    
    this._dataSource = dataSourceBuilder.Build();
  }

  public async Task<IDbConnection> AddCreationAsync(CancellationToken cancellationToken = default)
  {
    return await _dataSource.OpenConnectionAsync(cancellationToken);
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (_disposed)
      return;

    if (disposing)
    {
      _dataSource.Dispose();
    }

    _disposed = true;
  }

  public async ValueTask DisposeAsync()
  {
    if (_disposed)
      return;

    await _dataSource.DisposeAsync();

    _disposed = true;

    GC.SuppressFinalize(this);
  }

}