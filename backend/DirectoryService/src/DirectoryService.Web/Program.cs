using DirectoryService.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<DirectoryServiceDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.MapHealthChecks("/api/health");

if (!app.Environment.IsProduction())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

await app.RunAsync();