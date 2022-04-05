using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.TestHelpers.TestModels.Abstractions;
using System.Collections.Generic;
using System.Text.Json;
using Xunit.Abstractions;

namespace DiyProjectCalc.TestHelpers.TestModels;

public class ProjectTestModel : TestCasesAppearInTestExplorer
{
    public Project Project { get; set; } = new Project();

    //properties for testing return values 
    //(none yet)

    public void AddBasicShapes(List<BasicShape> basicShapes)
    {
        foreach (var basicShape in basicShapes)
            Project.AddBasicShape(basicShape);
    }
    public void AddMaterials(List<Material> materials)
    {
        foreach (var material in materials)
            Project.AddMaterial(material);
    }

    //methods required for nicely displaying test names in Visual Studio Test Explorer
    public override string ToString() => base.ToString();

    public override void Serialize(IXunitSerializationInfo info)
    {
        base.Serialize(info);

        var json = JsonSerializer.Serialize(Project);
        info.AddValue("Project", json);
    }

    public override void Deserialize(IXunitSerializationInfo info)
    {
        base.Deserialize(info);

        Project = JsonSerializer.Deserialize<Project>(info.GetValue<string>("Project")) ?? new();
    }
}