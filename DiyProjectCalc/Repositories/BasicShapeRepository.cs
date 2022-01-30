using DiyProjectCalc.Data;
using DiyProjectCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Repositories;

public class BasicShapeRepository : IBasicShapeRepository
{
    private ApplicationDbContext _dbContext;

    public BasicShapeRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<BasicShape?> GetBasicShapeAsync(int basicShapeId)
    {
        return await _dbContext.BasicShapes
            .Include(b => b.Project)
            .FirstOrDefaultAsync(m => m.BasicShapeId == basicShapeId);
    }

    public async Task<IEnumerable<BasicShape>> GetBasicShapesForProjectAsync(int projectId)
    {
        return await _dbContext.BasicShapes.Include(b => b.Project)
            .Where(b => b.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<bool> BasicShapeExists(int basicShapeId)
    {
        return await _dbContext.BasicShapes.AnyAsync(e => e.BasicShapeId == basicShapeId);
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