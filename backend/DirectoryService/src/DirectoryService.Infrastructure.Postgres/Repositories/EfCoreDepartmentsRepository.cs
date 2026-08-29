using System.Data;
using DirectoryService.Core.Departments;
using DirectoryService.Domain.Departments;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class EfCoreDepartmentsRepository : IDepartmentsRepository
{
  private readonly DirectoryServiceDbContext _dbContext;

  public EfCoreDepartmentsRepository(DirectoryServiceDbContext dbContext)
  {
    this._dbContext = dbContext;
  }
  
  public async Task<Guid> AddAsync(
    Department department,
    IEnumerable<DepartmentLocation> departmentLocations,
    CancellationToken cancellationToken = default)
  {
    using var transaction = await this._dbContext.Database.BeginTransactionAsync(cancellationToken);

    try
    {
      await this._dbContext.AddAsync(department, cancellationToken);
      
      await this._dbContext.AddRangeAsync(departmentLocations, cancellationToken);
    
      await this._dbContext.SaveChangesAsync(cancellationToken);
      
      await transaction.CommitAsync(cancellationToken);
    
      return department.Id;
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  public Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }

  public async Task<Department?> GetByIdAsync(Guid? departmentId, CancellationToken cancellationToken = default)
  {
    Department? department = await this._dbContext.Departments
      .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

    return department;
  }

  public Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default)
  {
    throw new DataException();
  }
}