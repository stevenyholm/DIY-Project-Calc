using DiyProjectCalc.Data;
using DiyProjectCalc.Models;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.Repositories;

public class EFMaterialRepository : IMaterialRepository
{
    private ApplicationDbContext _dbContext;

    public EFMaterialRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Material?> GetMaterialAsync(int expectedId)
    {
        return await _dbContext.Materials
            .Include(m => m.Project)
            .Include(m => m.BasicShapes)
            .FirstOrDefaultAsync(m => m.MaterialId == expectedId);

        //Material - Edit - GET needed this: 
    //    model.Material = await _context.Materials
    //.Include(m => m.BasicShapes)
    //.AsNoTracking()
    //.FirstOrDefaultAsync(m => m.MaterialId == id);

    }

    public async Task<IEnumerable<Material>> GetMaterialsForProjectAsync(int projectId)
    {
        return await _dbContext.Materials
            .Include(m => m.Project)
            .Include(m => m.BasicShapes)
            .Where(b => b.ProjectId == projectId)
            .ToListAsync();
    }


    public async Task<bool> MaterialExists(int materialId)
    {
        return await _dbContext.Materials.AnyAsync(e => e.MaterialId == materialId);
    }

    public async Task AddAsync(Material entity)
    {
        await _dbContext.Materials.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Material entity, int[] selectedBasicShapeIds)
    {
        var materialToSave = _dbContext.Materials
            .Include(m => m.BasicShapes)
            .FirstOrDefault(m => m.MaterialId == entity.MaterialId);

        if (materialToSave is not null) //TODO: change "== null" to "is null" across the project
        {
            _dbContext.Entry(materialToSave).CurrentValues.SetValues(entity);
            await SetBasicShapesForMaterial(materialToSave, selectedBasicShapeIds);

            _dbContext.Update(materialToSave);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new DbUpdateConcurrencyException();
        }
    }

    public async Task DeleteAsync(Material entity)
    {
        _dbContext.Materials.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    private async Task SetBasicShapesForMaterial(Material model, int[] selectedBasicShapeIds)
    {
        var basicShapeRepository = new EFBasicShapeRepository(_dbContext);
        var allBasicShapesForProject = await basicShapeRepository.GetBasicShapesForProjectAsync(model.ProjectId);

        var basicShapesToAdd = allBasicShapesForProject
            .Where(b => selectedBasicShapeIds.Any(s => s == b.BasicShapeId))
            .Where(b => !model.BasicShapes.Any(m => m.BasicShapeId == b.BasicShapeId));
        foreach (var basicShape in basicShapesToAdd)
        {
            model.BasicShapes.Add(basicShape);
        }

        bool isEditView = (model.MaterialId != default(int));
        if (isEditView)
        {
            var basicShapesToRemove = model.BasicShapes
                .Where(b => !selectedBasicShapeIds.Any(s => s == b.BasicShapeId)).ToList();
            foreach (var basicShape in basicShapesToRemove)
            {
                model.BasicShapes.Remove(basicShape);
            }
        }
    }

}