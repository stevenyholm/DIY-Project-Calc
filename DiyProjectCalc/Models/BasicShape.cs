using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiyProjectCalc.Models;

public enum BasicShapeType
{
    Rectangle = 1,
    Triangle = 2,
    Curved = 3
}

public class BasicShape 
{
    
    public int BasicShapeId { get; set; } 

    [Display(Name = "Type of Shape")]
    [EnumDataType(typeof(BasicShapeType))]
    public BasicShapeType ShapeType { get; set; }

    [Display(Name = "Side 1 / radius")]
    public int Number1 { get; set; }

    [Display(Name = "Side 2 / degrees")] //TODO: move view annotations to a ViewModel, make this a pure domain model object
    public int Number2 { get; set; }

    public string? Name { get; set; }

    [ForeignKey("Project")] //TODO: move data annotation to the context class, make this a pure domain model object
    public int ProjectId { get; set; }
    [ValidateNever]
    public virtual Project Project { get; set; } = null!;

    public void Edit(BasicShapeType shapeType, int number1, int number2, string name)
    {
        this.ShapeType = shapeType;
        this.Number1 = number1;
        this.Number2 = number2;
        this.Name = name;
    }

    public int Area() => ShapeType switch
    {
        BasicShapeType.Rectangle => RectangleArea(Number1, Number2),
        BasicShapeType.Triangle => TriangleArea(Number1, Number2),
        BasicShapeType.Curved => CurvedArea(Number1, Number2),
        _ => throw new ArgumentOutOfRangeException()
    };

    //============     Helper methods     =========================================================
    private static int RectangleArea(int side1, int side2) => side1 * side2;
    private static int TriangleArea(int side1, int side2) => side1 * side2 / 2;
    private static int CurvedArea(int radius, int degrees) => (int)( (radius * radius * Math.PI) / (degrees / 360) );
}
