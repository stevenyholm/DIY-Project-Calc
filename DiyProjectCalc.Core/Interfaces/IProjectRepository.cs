using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Core.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetProjectAsync(int id);
    Task<Project?> GetProjectWithBasicShapesAsync(int id);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task AddAsync(Project entity);
    Task UpdateAsync(Project entity);
    Task DeleteAsync(Project entity);
}
