using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetProjectAsync(int projectId);
    Task<Project?> GetProjectWithBasicShapesAsync(int projectId);
    Task<IEnumerable<Project>> GetAllProjectsAsync();
    Task AddAsync(Project entity);
    Task UpdateAsync(Project entity);
    Task DeleteAsync(Project entity);
}
