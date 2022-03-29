using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Core.Interfaces;

public interface IBasicShapeRepository
{
    Task<BasicShape?> GetBasicShapeAsync(int id);
    Task<IEnumerable<BasicShape>> GetBasicShapesForProjectAsync(int projectId);
    Task<bool> BasicShapeExists(int id);
    Task AddAsync(BasicShape entity);
    Task UpdateAsync(BasicShape entity);
    Task DeleteAsync(BasicShape entity);
}