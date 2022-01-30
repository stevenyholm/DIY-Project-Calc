using DiyProjectCalc.Data;
using DiyProjectCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Repositories;

public class EFProjectRepository : IProjectRepository
{
    private ApplicationDbContext _dbContext;

    public EFProjectRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Project?> GetProjectAsync(int projectId)
    {
        return await _dbContext.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
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
