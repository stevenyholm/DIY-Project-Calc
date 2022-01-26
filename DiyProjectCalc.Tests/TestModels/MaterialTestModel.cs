using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.TestModels.Abstractions;
using System.Text.Json;
using Xunit.Abstractions;

namespace DiyProjectCalc.Tests.TestModels;

public class MaterialTestModel : TestCasesAppearInTestExplorer
{
    public Material Material { get; set; } = new Material();

    //properties for testing return values 
    public double ExpectedDistanceNeeded;
    public double ExpectedAreaNeeded;
    public double ExpectedVolumeNeeded;
    public double ExpectedQuantityNeeded;
    public bool ExpectedCanCalculateQuantity;


    //methods required for nicely displaying test names in Visual Studio Test Explorer
    public override string ToString() => $"{base.TestCaseName}: {Material.MeasurementType} ({Material.Length}, {Material.Width}, {Material.Depth})";

    public override void Serialize(IXunitSerializationInfo info)
    {
        base.Serialize(info);

        info.AddValue(nameof(ExpectedDistanceNeeded), ExpectedDistanceNeeded, typeof(double));
        info.AddValue(nameof(ExpectedAreaNeeded), ExpectedAreaNeeded, typeof(double));
        info.AddValue(nameof(ExpectedVolumeNeeded), ExpectedVolumeNeeded, typeof(double));
        info.AddValue(nameof(ExpectedQuantityNeeded), ExpectedQuantityNeeded, typeof(double));
        info.AddValue(nameof(ExpectedCanCalculateQuantity), ExpectedCanCalculateQuantity, typeof(bool));

        var json = JsonSerializer.Serialize(Material);
        info.AddValue("Material", json);
    }

    public override void Deserialize(IXunitSerializationInfo info)
    {
        base.Deserialize(info);

        ExpectedDistanceNeeded = info.GetValue<double>(nameof(ExpectedDistanceNeeded));
        ExpectedAreaNeeded = info.GetValue<double>(nameof(ExpectedAreaNeeded));
        ExpectedVolumeNeeded = info.GetValue<double>(nameof(ExpectedVolumeNeeded));
        ExpectedQuantityNeeded = info.GetValue<double>(nameof(ExpectedQuantityNeeded));
        ExpectedCanCalculateQuantity = info.GetValue<bool>(nameof(ExpectedCanCalculateQuantity));

        Material = JsonSerializer.Deserialize<Material>(info.GetValue<string>("Material")) ?? new();
    }
}