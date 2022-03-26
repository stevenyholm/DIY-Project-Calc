using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Models.DTO;

public record BasicShapeDTO(
    BasicShapeType ShapeType,
    string? Name,
    double Number1,
    double Number2,
    int BasicShapeId = default,
    string Description = "",
    double Area = default,
    double Distance = default,
    string ProjectName = "",
    int ProjectId = default
    )
{
}
