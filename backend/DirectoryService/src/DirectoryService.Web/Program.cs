using System.Data;
using DirectoryService.Core;
using DirectoryService.Core.Departments;
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
  case "Dapper":
  {
    builder.Services.AddScoped<IDbConnectionFactory, NpgSqlConnectionFactory>();
    builder.Services.AddScoped<ILocationsRepository, NpgSqlLocationsRepository>();
    builder.Services.AddScoped<IDepartmentsRepository, NpgsqlDepartmentsRepository>();
    break;
  }
  default:
  {
    builder.Services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
    builder.Services.AddScoped<IDepartmentsRepository, EfCoreDepartmentsRepository>();
    break;
  }
}

builder.Services.AddScoped<CreateLocationValidator>();

builder.Services.AddScoped<LocationService>();

builder.Services.AddScoped<CreateDepartmentValidator>();

builder.Services.AddScoped<DepartmentsService>();


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