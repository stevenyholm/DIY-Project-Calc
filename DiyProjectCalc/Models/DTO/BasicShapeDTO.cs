using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.SharedKernel;

namespace DiyProjectCalc.Models.DTO;

public record BasicShapeDTO(
    BasicShapeType ShapeType,
    string? Name,
    double Number1,
    double Number2,
    int Id = default,
    string Description = "",
    double Area = default,
    double Distance = default,
    string ProjectName = ""
    ) : BaseDTO(Id)
{
}
