using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Core.Interfaces;

public interface IMaterialRepository
{
    Task<Material?> GetMaterialAsync(int id);
    Task<IEnumerable<Material>> GetMaterialsForProjectAsync(int projectId);
    Task<bool> MaterialExists(int id);
    Task AddAsync(Material entity);
    Task UpdateAsync(Material entity, int[] selectedBasicShapeIds);
    Task DeleteAsync(Material entity);
}