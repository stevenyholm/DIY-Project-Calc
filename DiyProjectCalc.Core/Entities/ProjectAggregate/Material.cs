using System.ComponentModel.DataAnnotations;
using DiyProjectCalc.SharedKernel;
//TODO: using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DiyProjectCalc.Core.Entities.ProjectAggregate;

public class Material : BaseEntity
{
    [Display(Name = "Type of Measurement")]
    [EnumDataType(typeof(MaterialMeasurement))]
    public MaterialMeasurement MeasurementType { get; set; }

    public string? Name { get; set; }

    private List<BasicShape> _basicShapes = new List<BasicShape>();
    public IEnumerable<BasicShape> BasicShapes => _basicShapes.AsReadOnly(); 

    //size of material 
    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Length { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Width { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? Depth { get; set; }

    [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
    public double? DepthNeeded { get; set; }


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

    public double DistanceNeeded() => this.BasicShapes.Select(a => a.Distance).Sum();
    public double AreaNeeded() => this.BasicShapes.Select(a => a.Area).Sum();
    public double VolumeNeeded() => this.AreaNeeded() * this.DepthNeeded ?? 1;

    public void AddBasicShape(BasicShape newBasicShape)
    {
        _basicShapes.Add(newBasicShape);
    }
    public void RemoveBasicShape(BasicShape basicShapeToDelete)
    {
        _basicShapes.Remove(basicShapeToDelete!);
    }

    private bool IsMeasurementValid(double? measurement) 
        => measurement != null && measurement.HasValue && measurement > 0;

    private double MaterialDistance() => this.Length ?? 1.0;
    private double MaterialArea() => (this.Length ?? 1) * (this.Width ?? 1);
    private double MaterialVolume() => (this.Length ?? 1) * (this.Width ?? 1) * (this.Depth ?? 1);

}

