using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.TestModels.Abstractions;
using System.Text.Json;
using Xunit.Abstractions;

namespace DiyProjectCalc.Tests.TestModels;

public class ProjectTestModel : TestCasesAppearInTestExplorer
{
    public Project Project { get; set; } = new Project();

    //properties for testing return values 
    //(none yet)


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