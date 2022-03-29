using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Core.Interfaces;

public interface IBasicShapeRepository
{
    Task<BasicShape?> GetBasicShapeAsync(int basicShapeId);
    Task<IEnumerable<BasicShape>> GetBasicShapesForProjectAsync(int projectId);
    Task<bool> BasicShapeExists(int basicShapeId);
    Task AddAsync(BasicShape entity);
    Task UpdateAsync(BasicShape entity);
    Task DeleteAsync(BasicShape entity);
}