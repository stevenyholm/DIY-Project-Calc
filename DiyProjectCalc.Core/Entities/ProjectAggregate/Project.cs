
using DiyProjectCalc.SharedKernel;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate;

public class Project : BaseAggregateRoot
{
    [Required]
    [Display(Name = "Project Name")]
    public string? Name { get; set; }

    public virtual ICollection<BasicShape> BasicShapes { get; set; } = new HashSet<BasicShape>();
    public virtual ICollection<Material> Materials { get; set; } = new HashSet<Material>();

    //========================== BasicShape aggregate entity =========================
    public BasicShape GetBasicShape(int basicShapeId)
        => GetAggregate<BasicShape>(BasicShapes, basicShapeId);
    public bool BasicShapeExists(int basicShapeId)
        => AggregateExists<BasicShape>(BasicShapes, basicShapeId);
    public void AddBasicShape(BasicShape newBasicShape) 
        => AddAggregate<BasicShape>(BasicShapes, newBasicShape);
    public void UpdateBasicShape(BasicShape detachedBasicShapeWithUpdate, IMapper mapper)
    {
        UpdateAggregate<BasicShape>(BasicShapes, detachedBasicShapeWithUpdate, mapper);
    }
    public void RemoveBasicShape(int basicShapeId)
        => RemoveAggregate<BasicShape>(BasicShapes, basicShapeId);


    //========================== Material aggregate entity =========================
    public Material GetMaterial(int materialId)
        => GetAggregate<Material>(Materials, materialId);
    public bool MaterialExists(int materialId)
        => AggregateExists<Material>(Materials, materialId);
    public void AddMaterial(Material newMaterial)
        => AddAggregate<Material>(Materials, newMaterial);
    public void UpdateMaterial(Material detachedMaterialWithUpdate, IMapper mapper)
    {
        UpdateAggregate<Material>(Materials, detachedMaterialWithUpdate, mapper);
    }
    public void RemoveMaterial(int materialId)
        => RemoveAggregate<Material>(Materials, materialId);


}