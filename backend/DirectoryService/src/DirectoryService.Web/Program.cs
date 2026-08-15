using System.Data;
using DirectoryService.Core;
using DirectoryService.Core.Locations;
using DirectoryService.Infrastructure.Postgres;
using DirectoryService.Infrastructure.Postgres.Database;
using DirectoryService.Infrastructure.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<DirectoryServiceDbContext>(options =>
{
  options
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLoggerFactory(DirectoryServiceDbContext.CreateLoggerFactory());
});

var defaultRepository = builder.Configuration["DefaultRepository"];

switch (defaultRepository)
{
  case "EfCore":
  {
    builder.Services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
    break;
  }
  case "Dapper":
  {
    builder.Services.AddScoped<ILocationsRepository, NpgSqlLocationsRepository>();
    break;
  }
  default:
  {
    builder.Services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
    break;
  }
}

builder.Services.AddScoped<IDbConnectionFactory, NpgSqlConnectionFactory>();

builder.Services.AddScoped<CreateLocationValidator>();

builder.Services.AddScoped<LocationService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.MapControllers();

app.MapHealthChecks("/api/health");

if (!app.Environment.IsProduction())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

await app.RunAsync();