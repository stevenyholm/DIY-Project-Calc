
namespace DiyProjectCalc.Models.DTO;

public record ProjectDTOWithBasicShapes(
    int Id, 
    string? Name
    )
{
    public IEnumerable<BasicShapeDTO> BasicShapes { get; set; } = Enumerable.Empty<BasicShapeDTO>();
}