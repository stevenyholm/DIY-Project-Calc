using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiyProjectCalc.Models;

public enum MaterialMeasurement
{
    Linear = 1,
    Area = 2,
    Volume = 3
}

public class Material
{
    public int MaterialId { get; set; }

    [Display(Name = "Type of Measurement")]
    [EnumDataType(typeof(MaterialMeasurement))]
    public MaterialMeasurement MeasurementType { get; set; }

    public string? Name { get; set; }

    //navigation properties 
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    [ValidateNever]
    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<BasicShape> BasicShapes { get; set; } = new HashSet<BasicShape>();


    //size of material 
    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Length { get; set; } 

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Width { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Depth { get; set; }


    public double Area() => BasicShapes.Select(a => a.Area).Sum();
    public double Distance() => BasicShapes.Select(a => a.Distance).Sum();
    public double Volume() => this.Area() * this.Depth??1;

}
