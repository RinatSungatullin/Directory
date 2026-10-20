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

  public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    List<Department> departments = await this._dbContext.Departments.ToListAsync(cancellationToken);

    return departments;
  }

  public async Task<Department?> GetByIdAsync(Guid? departmentId, CancellationToken cancellationToken = default)
  {
    Department? department = await this._dbContext.Departments
      .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

    return department;
  }

  public async Task<Guid> UpdateAsync(
    Department department,
    CancellationToken cancellationToken = default)
  {
    await this._dbContext.SaveChangesAsync(cancellationToken);
    
    return department.Id;
  }

  public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken = default)
  {
    await this._dbContext.Departments
      .Where(d => d.Id == departmentId)
      .ExecuteDeleteAsync(cancellationToken);
    
    return departmentId;
  }

  public async Task<DepartmentLocation?> GetDepartmentLocationByDepartmentIdAndLocationIdAsync(
    Guid departmentId,
    Guid locationId,
    CancellationToken cancellationToken = default)
  {
    DepartmentLocation? departmentLocation = await this._dbContext.DepartmentLocation
      .FirstOrDefaultAsync(
        dl => dl.DepartmentId == departmentId  && dl.LocationId == locationId,
        cancellationToken);

    return departmentLocation;
  }


  public async Task<Guid> DeleteDepartmentLocationAsync(
    Guid departmentLocationId,
    CancellationToken cancellationToken = default)
  {
    await this._dbContext.DepartmentLocation
      .Where(x => x.Id == departmentLocationId)
      .ExecuteDeleteAsync(cancellationToken);
    
    await this._dbContext.SaveChangesAsync(cancellationToken);
    
    return departmentLocationId;
  }
  
  public async Task<Guid> AddDepartmentLocationAsync(
    DepartmentLocation departmentLocation,
    CancellationToken cancellationToken = default)
  {
    await this._dbContext.AddAsync(departmentLocation, cancellationToken);
    
    await this._dbContext.SaveChangesAsync(cancellationToken);
    
    return departmentLocation.Id;
  }
  
}