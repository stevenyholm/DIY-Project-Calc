using System.Collections;
using System.Collections.Generic;

namespace DiyProjectCalc.Tests.TestData.Abstractions;

public class ParameterizedTestClassData : IEnumerable<object[]>
{
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public virtual IEnumerator<object[]> GetEnumerator() { return null!; }
}
