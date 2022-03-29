using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Core.Interfaces;

namespace DiyProjectCalc.Infrastructure.Repositories;

public class EFProjectRepository : IProjectRepository
{
    private ApplicationDbContext _dbContext;

    public EFProjectRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Project?> GetProjectAsync(int id)
    {
        return await _dbContext.Projects.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Project?> GetProjectWithBasicShapesAsync(int id)
    {
        return await _dbContext.Projects.Where(p => p.Id == id).Include(p => p.BasicShapes).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _dbContext.Projects.Include(p => p.BasicShapes).ToListAsync();
    }

    public async Task AddAsync(Project entity)
    {
        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project entity)
    {
        _dbContext.Projects.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Project entity)
    {
        _dbContext.Projects.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
