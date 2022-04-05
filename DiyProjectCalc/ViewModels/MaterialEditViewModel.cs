using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.ViewModels;

public class MaterialEditViewModel
{
    public int ProjectId { get; set; }

    public Material? Material { get; set; }

    public IEnumerable<BasicShape> BasicShapesForProject { get; set; } = new HashSet<BasicShape>(); 
   
    public List<(int BasicShapeId, string Description, bool Selected)> BasicShapesData() =>
        BasicShapesForProject.ToList()
            .Select(b => (b.Id, b.Description, MaterialHasBasicShape(this.Material, b)) ) 
            .ToList();

    private bool MaterialHasBasicShape(Material? material, BasicShape basicShape) => 
        (material is not null) && (material.BasicShapes.Any(b => b.Id == basicShape.Id)) 
        ? true : false;
}
