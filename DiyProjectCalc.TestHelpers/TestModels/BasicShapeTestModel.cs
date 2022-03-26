using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.TestHelpers.TestModels.Abstractions;
using System.Text.Json;
using Xunit.Abstractions;

namespace DiyProjectCalc.TestHelpers.TestModels;

public class BasicShapeTestModel : TestCasesAppearInTestExplorer
{
    public BasicShape BasicShape { get; set; } = new BasicShape();

    //properties for testing return values 
    public double ExpectedArea;
    public double ExpectedDistance;
    public string ExpectedDescription { get => $"{BasicShape.Name}, {BasicShape.ShapeType} ({BasicShape.Number1}, {BasicShape.Number2})"; }

    //methods required for nicely displaying test names in Visual Studio Test Explorer
    public override string ToString() => $"{base.TestCaseName}: {BasicShape.ShapeType} ({BasicShape.Number1}, {BasicShape.Number2})";

    public override void Serialize(IXunitSerializationInfo info)
    {
        base.Serialize(info);

        info.AddValue(nameof(ExpectedArea), ExpectedArea, typeof(double));
        info.AddValue(nameof(ExpectedDistance), ExpectedDistance, typeof(double));

        var json = JsonSerializer.Serialize(BasicShape);
        info.AddValue("BasicShape", json);

    }

    public override void Deserialize(IXunitSerializationInfo info)
    {
        base.Deserialize(info);

        ExpectedArea = info.GetValue<double>(nameof(ExpectedArea));
        ExpectedDistance = info.GetValue<double>(nameof(ExpectedDistance));

        BasicShape = JsonSerializer.Deserialize<BasicShape>(info.GetValue<string>("BasicShape")) ?? new();

    }
}

