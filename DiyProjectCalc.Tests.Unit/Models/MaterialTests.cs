using System.Linq;
using FluentAssertions;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestModels;

namespace DiyProjectCalc.Tests.Unit.Models;

public class MaterialTests
{
    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    [Trait("DistanceNeeded", "")]
    public void ValidInput_Returns_CorrectValue_For_DistanceNeeded(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.DistanceNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedDistanceNeeded, MaterialTestData.TestPrecision, 
            because: "(debug tip: check distance calculations of all child BasicShapes");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    [Trait("AreaNeeded", "")]
    public void ValidInput_Returns_CorrectValue_For_AreaNeeded(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.AreaNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedAreaNeeded, MaterialTestData.TestPrecision,
            because: "(debug tip: check area calculations of all child BasicShapes");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    [Trait("VolumeNeeded", "")]
    public void ValidInput_Returns_CorrectValue_For_VolumeNeeded(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.VolumeNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedVolumeNeeded, MaterialTestData.TestPrecision,
            because: "(debug tip: check area calculations of all child BasicShapes");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    [Trait("QuantityNeeded", "")]
    public void ValidInput_Returns_CorrectValue_For_QuantityNeeded(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.QuantityNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedQuantityNeeded, MaterialTestData.TestPrecision);
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    [Trait("CanCalculateQuantity", "")]
    public void ValidInput_Returns_CorrectValue_For_CanCalculateQuantity(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.CanCalculateQuantity();

        //Assert
        result.Should().Be(testData.ExpectedCanCalculateQuantity);
    }

    [Fact]
    [Trait("AddBasicShape", "")]
    public void ValidInput_Adds_Item_For_AddBasicShape()
    {
        //Arrange
        var material = MaterialTestData.MockSimpleMaterial;
        var expectedCount = material.BasicShapes.Count + 1;
        var basicShapeToAdd = BasicShapeTestData.NewBasicShape;

        //Act
        material.AddBasicShape(basicShapeToAdd);

        //Assert
        var afterCount = material.BasicShapes.Count;
        afterCount.Should().Be(expectedCount);
    }

    [Fact]
    [Trait("RemoveBasicShape", "")]
    public void ValidInput_Removes_Item_For_RemoveBasicShape()
    {
        //Arrange
        var material = MaterialTestData.MockMaterialWithBasicShapes;
        var expectedCount = material.BasicShapes.Count - 1;
        var basicShapeToRemove = material.BasicShapes.First();

        //Act
        material.RemoveBasicShape(basicShapeToRemove);

        //Assert
        var afterCount = material.BasicShapes.Count;
        afterCount.Should().Be(expectedCount);
    }
}
