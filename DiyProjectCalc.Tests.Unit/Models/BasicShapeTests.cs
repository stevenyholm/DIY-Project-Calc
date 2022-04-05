using FluentAssertions;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestModels;
using System.Linq;

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

    [Fact]
    [Trait("AddMaterial", "")]
    public void ValidInput_Adds_Item_For_AddMaterial()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        var expectedCount = basicShape.Materials.Count() + 1;
        var materialToAdd = MaterialTestData.NewMaterial;

        //Act
        basicShape.AddMaterial(materialToAdd);

        //Assert
        var afterCount = basicShape.Materials.Count();
        afterCount.Should().Be(expectedCount);
    }

    [Fact]
    [Trait("RemoveMaterial", "")]
    public void ValidInput_Removes_Item_For_RemoveMaterial()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShapeWithMaterials;
        var expectedCount = basicShape.Materials.Count() - 1;
        var materialToRemove = basicShape.Materials.First();

        //Act
        basicShape.RemoveMaterial(materialToRemove);

        //Assert
        var afterCount = basicShape.Materials.Count();
        afterCount.Should().Be(expectedCount);
    }
}
