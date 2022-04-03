using Ardalis.Specification;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
public class ProjectWithBasicShapesSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectWithBasicShapesSpec(int projectId)
    {
        Query
            .Where(project => project.Id == projectId)
            .Include(project => project.BasicShapes);
    }
}