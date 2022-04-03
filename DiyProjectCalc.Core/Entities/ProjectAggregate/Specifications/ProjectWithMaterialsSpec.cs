using Ardalis.Specification;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
public class ProjectWithMaterialsSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectWithMaterialsSpec(int projectId)
    {
        Query
            .Where(project => project.Id == projectId)
            .Include(project => project.Materials);
    }
}