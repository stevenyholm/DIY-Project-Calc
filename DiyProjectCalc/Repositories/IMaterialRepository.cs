using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Repositories;

public interface IMaterialRepository
{
    Task<Material?> GetMaterialAsync(int expectedId);
    Task<IEnumerable<Material>> GetMaterialsForProjectAsync(int projectId);
    Task<bool> MaterialExists(int materialId);
    Task AddAsync(Material entity);
    Task UpdateAsync(Material entity, int[] selectedBasicShapeIds);
    Task DeleteAsync(Material entity);
}