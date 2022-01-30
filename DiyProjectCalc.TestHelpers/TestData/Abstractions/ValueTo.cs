using DiyProjectCalc.TestHelpers.TestModels.Abstractions;
using Xunit.Abstractions;

namespace DiyProjectCalc.TestHelpers.TestData.Abstractions;

public static class ValueTo
{
    public static object[] Test<T>(T value) => new object[] { new ValueWrapper<T>() { Value = value } };
}

public class ValueWrapper<T> : TestCasesAppearInTestExplorer
{
    public T Value { get; set; } = default(T)!;

    public override string ToString() => $"{Value}";

    public override void Serialize(IXunitSerializationInfo info)
    {
        base.Serialize(info);

        info.AddValue(nameof(Value), Value, typeof(T));
    }

    public override void Deserialize(IXunitSerializationInfo info)
    {
        base.Serialize(info);

        Value = info.GetValue<T>(nameof(Value));
    }
}
