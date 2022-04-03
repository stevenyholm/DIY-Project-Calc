using FluentAssertions;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestModels;

namespace DiyProjectCalc.Tests.Unit.Models;

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
        result.Should().BeApproximately(testData.ExpectedArea, BasicShapeTestData.TestPrecision,
            because: "this tested a calculation with valid input (see test name for input values)");
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
        result.Should().BeApproximately(testData.ExpectedDistance, BasicShapeTestData.TestPrecision,
            because: "this tested a calculation with valid input (see test name for input values)");
    }

    [Theory]
    [ClassData(typeof(BasicShapeValidClassData))]
    [Trait("Description ", "")]
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
