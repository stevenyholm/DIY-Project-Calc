using System.Collections.Generic;
using DiyProjectCalc.TestHelpers.TestData.Abstractions;
using DiyProjectCalc.TestHelpers.TestModels;

namespace DiyProjectCalc.TestHelpers.TestData;

public class BasicShapeValidClassData : ParameterizedTestClassData
{
    public override IEnumerator<object[]> GetEnumerator() =>
        base.GetEnumerator<BasicShapeTestModel>(new BasicShapeTestData().ValidBasicShapeTestModelList);
}