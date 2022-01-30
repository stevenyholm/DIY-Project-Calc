using Xunit.Abstractions;

namespace DiyProjectCalc.TestHelpers.TestModels.Abstractions;

public class TestCasesAppearInTestExplorer : IXunitSerializable  //required for Visual Studio Test Explorer
{
    public TestCasesAppearInTestExplorer() { } //empty constructor required for IXunitSerializable

    public string? TestCaseName;

    public override string ToString() => $"{TestCaseName}";  //required for Visual Studio Test Explorer

    public virtual void Serialize(IXunitSerializationInfo info)  //required for IXunitSerializable
    {
        info.AddValue(nameof(TestCaseName), TestCaseName, typeof(string));
    }

    public virtual void Deserialize(IXunitSerializationInfo info)  //required for IXunitSerializable
    {
        TestCaseName = info.GetValue<string?>(nameof(TestCaseName));
    }
}