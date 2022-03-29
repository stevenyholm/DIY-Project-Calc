using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using DiyProjectCalc.Core.Interfaces;

namespace DiyProjectCalc.Infrastructure.Repositories;

public class EFBasicShapeRepository : IBasicShapeRepository
{
    private ApplicationDbContext _dbContext;

    public EFBasicShapeRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<BasicShape?> GetBasicShapeAsync(int id)
    {
        return await _dbContext.BasicShapes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<BasicShape>> GetBasicShapesForProjectAsync(int projectId)
    {
        return await _dbContext.BasicShapes.Include(b => b.Project)
            .Where(b => b.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<bool> BasicShapeExists(int id)
    {
        return await _dbContext.BasicShapes.AnyAsync(e => e.Id == id);
    }

    public async Task AddAsync(BasicShape entity)
    {
        await _dbContext.BasicShapes.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(BasicShape entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(BasicShape entity)
    {
        _dbContext.BasicShapes.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}