using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Postgres.Database;

public class NpgSqlConnectionFactory : IDisposable, IAsyncDisposable
{
  private readonly NpgsqlDataSource _dataSource;

  public NpgSqlConnectionFactory(IConfiguration configuration, ILoggerFactory loggerFactory)
  {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(
      configuration.GetConnectionString("DefaultConnection"))
      .UseLoggerFactory(loggerFactory);
    
    this._dataSource = dataSourceBuilder.Build();
  }

  public async Task<NpgsqlConnection> CreateConnection()
  {
    return await _dataSource.OpenConnectionAsync();
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing)
    {
      this._dataSource.Dispose();
    }
  }

  public async ValueTask DisposeAsync()
  {
    Dispose(true);
    
    GC.SuppressFinalize(this);
  }
}