using Xunit;
using FluentAssertions;
using DiyProjectCalc.Tests.TestData;
using DiyProjectCalc.Tests.TestModels;

namespace DiyProjectCalc.Tests.ModelTests;

public class BasicShapeTests
{
    [Theory]
    [ClassData(typeof(BasicShapeValidClassData))]
    public void Area_ValidInput_CorrectResult(BasicShapeTestModel testData)
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
    public void Distance_ValidInput_CorrectResult(BasicShapeTestModel testData)
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
    public void Description_ValidInput_CorrectResult(BasicShapeTestModel testData)
    {
        //Arrange
        var basicShape = testData.BasicShape;

        //Act
        var result = basicShape.Description;

        //Assert
        result.Should().Be(testData.ExpectedDescription);
    }

}
