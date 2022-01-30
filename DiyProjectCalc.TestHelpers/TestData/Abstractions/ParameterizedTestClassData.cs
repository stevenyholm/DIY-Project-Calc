using System.Collections;
using System.Collections.Generic;

namespace DiyProjectCalc.TestHelpers.TestData.Abstractions;

public class ParameterizedTestClassData : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual IEnumerator<object[]> GetEnumerator() { return null!; }
    public IEnumerator<object[]> GetEnumerator<T>(ICollection<T> testData)
    {
        foreach (var testModel in testData)
        {
            yield return ObjectTo.Test(testModel!);
        }
    }
}
