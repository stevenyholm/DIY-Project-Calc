using DiyProjectCalc.Models;
//TODO: change ViewModels to use simple types and not reference Model classes (domain logic)
//      which would make for a cleaner architecture, but a more complicated project 
//      and consider using AutoMapper https://automapper.org/ 
//      and a flattened ViewModel allows for this Bind attribute on Create: Create([Bind("")] ), a security best practice
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiyProjectCalc.ViewModels;

public class MaterialEditViewModel
{
    public int ProjectId { get; set; }

    private Material? _material;
    public Material? Material 
    {
        get => (_material is null)
                ? null 
                : FixMaterialProjectId(_material, this.ProjectId);
        set => _material = value; 
    } 
    private Material FixMaterialProjectId(Material material, int projectId)
    {
        if (material.ProjectId == default(int))
            material.ProjectId = projectId;
        return material;
    }

    public ICollection<BasicShape> BasicShapesForProject { get; set; } = new HashSet<BasicShape>(); 
   
    public List<(int BasicShapeId, string Description, bool Selected)> BasicShapesData() =>
        BasicShapesForProject.ToList()
            .Select(b => (b.BasicShapeId, b.Description, MaterialHasBasicShape(this.Material, b)) ) 
            .ToList();

    private bool MaterialHasBasicShape(Material? material, BasicShape basicShape) => 
        (material is not null) && (material.BasicShapes.Any(b => b.BasicShapeId == basicShape.BasicShapeId)) 
        ? true : false;
}
