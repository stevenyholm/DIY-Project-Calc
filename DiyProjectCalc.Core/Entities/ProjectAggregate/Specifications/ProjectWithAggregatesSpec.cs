using Ardalis.Specification;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
public class ProjectWithAggregatesSpec : Specification<Project>, ISingleResultSpecification
{
    public ProjectWithAggregatesSpec(int projectId)
    {
        Query
            .Where(project => project.Id == projectId)
            .Include(project => project.Materials)
            .ThenInclude(material => material.BasicShapes)
            .Include(project => project.BasicShapes)
            ;
    }
}