using System.ComponentModel.DataAnnotations;
using DiyProjectCalc.SharedKernel;
//TODO: using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate;



public class BasicShape : BaseEntity
{


    [Display(Name = "Type of Shape")]
    [EnumDataType(typeof(BasicShapeType))]
    public BasicShapeType ShapeType { get; set; }

    public string? Name { get; set; }

    private List<Material> _materials = new List<Material>();
    public IEnumerable<Material> Materials => _materials.AsReadOnly();

    //size of shape 
    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    [Display(Name = "Side 1 / radius")]
    public double Number1 { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    [Display(Name = "Side 2 / degrees")]
    public double Number2 { get; set; }

    public string Description { get => $"{Name}, {ShapeType} ({Number1}, {Number2})"; }


    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double Area
    {
        get => ShapeType switch
        {
            BasicShapeType.Rectangle => RectangleArea(Number1, Number2),
            BasicShapeType.Triangle => TriangleArea(Number1, Number2),
            BasicShapeType.Curved => CurvedArea(Number1, Number2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double Distance
    {
        get => ShapeType switch
        {
            BasicShapeType.Rectangle => StraightLineDistance(Number1),
            BasicShapeType.Triangle => AngleDistance(Number1, Number2),
            BasicShapeType.Curved => CurveDistance(Number1, Number2),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public void AddMaterial(Material newMaterial)
    {
        _materials.Add(newMaterial);
    }
    public void RemoveMaterial(Material materialToDelete)
    {
        _materials.Remove(materialToDelete!);
    }

    //============     Helper methods     =========================================================
    private static double RectangleArea(double side1, double side2) => side1 * side2;
    private static double TriangleArea(double side1, double side2) => side1 * side2 / 2.0;
    private static double CurvedArea(double radius, double degrees) => (radius * radius * Math.PI) * (degrees / 360.0);

    private static double StraightLineDistance(double distance) => distance;
    private static double AngleDistance(double side1, double side2) => Math.Sqrt((side1 * side1) + (side2 * side2));
    private static double CurveDistance(double radius, double degrees) => (2 * radius * Math.PI) * (degrees / 360.0);
}
