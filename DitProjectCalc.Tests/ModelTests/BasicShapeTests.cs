using Xunit;
using FluentAssertions;
using DiyProjectCalc.Tests.TestData;
using DiyProjectCalc.Tests.TestModels;

namespace DiyProjectCalc.Tests.ModelTests;

public class BasicShapeTests
{
    [Theory]
    [ClassData(typeof(BasicShapeValidClassData))]
    [Trait("Area", "")]
    public void ValidInput_Returns_CorrectValue_For_Area(BasicShapeTestModel testData)
    {
        //Arrange
        var basicShape = testData.BasicShape;

        //Act
        var result = basicShape.Area;

        //Assert
        result.Should().BeApproximately(testData.ExpectedArea, BasicShapesTestData.TestPrecision);
    }

    [Theory]
    [ClassData(typeof(BasicShapeValidClassData))]
    [Trait("Distance", "")]
    public void ValidInput_Returns_CorrectValue_For_Distance(BasicShapeTestModel testData)
    {
        //Arrange
        var basicShape = testData.BasicShape;

        //Act
        var result = basicShape.Distance;

        //Assert
        result.Should().BeApproximately(testData.ExpectedDistance, BasicShapesTestData.TestPrecision);
    }

    [Theory]
    [ClassData(typeof(BasicShapeValidClassData))]
    [Trait("Description", "")]
    public void ValidInput_Returns_CorrectValue_For_Description(BasicShapeTestModel testData)
    {
        //Arrange
        var basicShape = testData.BasicShape;

        //Act
        var result = basicShape.Description;

        //Assert
        result.Should().Be(testData.ExpectedDescription);
    }

}
