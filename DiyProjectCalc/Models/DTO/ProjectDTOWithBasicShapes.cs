using DiyProjectCalc.Models;

namespace DiyProjectCalc.Models.DTO;

public record ProjectDTOWithBasicShapes(
    int ProjectId, 
    string? Name
    )
{
    public IEnumerable<BasicShapeDTO> BasicShapes { get; set; } = Enumerable.Empty<BasicShapeDTO>();
}