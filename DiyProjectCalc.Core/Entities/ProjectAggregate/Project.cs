using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DiyProjectCalc.SharedKernel;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate;

public class Project : BaseAggregateRoot
{
    [Required]
    [Display(Name = "Project Name")]
    public string? Name { get; set; }

    private List<BasicShape> _basicShapes = new List<BasicShape>();
    public IEnumerable<BasicShape> BasicShapes => _basicShapes.AsReadOnly();

    private List<Material> _materials = new List<Material>();
    public IEnumerable<Material> Materials => _materials.AsReadOnly();


    //========================== BasicShape aggregate entity =========================
    public BasicShape GetBasicShape(int basicShapeId)
        => GetAggregate<BasicShape>(_basicShapes, basicShapeId);
    public bool BasicShapeExists(int basicShapeId)
        => AggregateExists<BasicShape>(_basicShapes, basicShapeId);
    public void AddBasicShape(BasicShape newBasicShape) 
        => AddAggregate<BasicShape>(_basicShapes, newBasicShape);
    public void UpdateBasicShape(BasicShape detachedBasicShapeWithUpdate, IMapper mapper)
    {
        UpdateAggregate<BasicShape>(_basicShapes, detachedBasicShapeWithUpdate, mapper);
    }
    public void RemoveBasicShape(int basicShapeId)
        => RemoveAggregate<BasicShape>(_basicShapes, basicShapeId);


    //========================== Material aggregate entity =========================
    public Material GetMaterial(int materialId)
        => GetAggregate<Material>(_materials, materialId);
    public bool MaterialExists(int materialId)
        => AggregateExists<Material>(_materials, materialId);
    public void AddMaterial(Material newMaterial)
        => AddAggregate<Material>(_materials, newMaterial);
    public void UpdateMaterial(Material detachedMaterialWithUpdate, IMapper mapper)
    {
        UpdateAggregate<Material>(_materials, detachedMaterialWithUpdate, mapper);
    }
    public void RemoveMaterial(int materialId)
        => RemoveAggregate<Material>(_materials, materialId);


}