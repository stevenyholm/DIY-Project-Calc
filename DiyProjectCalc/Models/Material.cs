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

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? DepthNeeded { get; set; }

    //TODO: separate commit: move TODO notes about features to Jira (and out of code)
    //TODO: implement a more sophisticated quantity needed that accounts for waste 

    public double QuantityNeeded() =>
        (!CanCalculateQuantity()) ? 0 :
        this.MeasurementType switch
        {
            MaterialMeasurement.Linear => DistanceNeeded() / MaterialDistance(),
            MaterialMeasurement.Area => AreaNeeded() / MaterialArea(),
            MaterialMeasurement.Volume => VolumeNeeded() / MaterialVolume(),
            _ => 0
        };

    public bool CanCalculateQuantity() => 
        this.MeasurementType switch
        {
            MaterialMeasurement.Linear => IsMeasurementValid(this.Length),
            MaterialMeasurement.Area => IsMeasurementValid(this.Length) && IsMeasurementValid(this.Width),
            MaterialMeasurement.Volume => IsMeasurementValid(this.Depth),
            _ => false
        };

    private bool IsMeasurementValid(double? measurement) => measurement != null && measurement.HasValue && measurement > 0;

    private double MaterialDistance() => this.Length ?? 1.0;
    private double MaterialArea() => (this.Length ?? 1) * (this.Width ?? 1);
    private double MaterialVolume() => (this.Length ?? 1) * (this.Width ?? 1) * (this.Depth ?? 1);
    public double DistanceNeeded() => this.BasicShapes.Select(a => a.Distance).Sum();
    public double AreaNeeded() => this.BasicShapes.Select(a => a.Area).Sum();
    public double VolumeNeeded() => this.AreaNeeded() * this.DepthNeeded??1;

}
