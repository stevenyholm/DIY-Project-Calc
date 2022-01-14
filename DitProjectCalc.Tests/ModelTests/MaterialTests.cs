using Xunit;
using FluentAssertions;
using DiyProjectCalc.Tests.TestData;
using DiyProjectCalc.Tests.TestModels;

namespace DiyProjectCalc.Tests.ModelTests;

public class MaterialTests
{
    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    public void DistanceNeeded_ValidInput_CorrectResult(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.DistanceNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedDistanceNeeded, MaterialsTestData.TestPrecision, because: "tested happy path: valid input should give correct output");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    public void AreaNeeded_ValidInput_CorrectResult(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.AreaNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedAreaNeeded, MaterialsTestData.TestPrecision, because: "tested happy path: valid input should give correct output");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    public void VolumeNeeded_ValidInput_CorrectResult(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.VolumeNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedVolumeNeeded, MaterialsTestData.TestPrecision, because: "tested happy path: valid input should give correct output");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    public void QuantityNeeded_ValidInput_CorrectResult(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.QuantityNeeded();

        //Assert
        result.Should().BeApproximately(testData.ExpectedQuantityNeeded, MaterialsTestData.TestPrecision, because: "tested happy path: valid input should give correct output");
    }

    [Theory]
    [ClassData(typeof(MaterialValidClassData))]
    public void CanCalculateQuantity_ValidInput_CorrectResult(MaterialTestModel testData)
    {
        //Arrange
        var material = testData.Material;

        //Act
        var result = material.CanCalculateQuantity();

        //Assert
        result.Should().Be(testData.ExpectedCanCalculateQuantity, because: "tested happy path: valid input should give correct output");
    }

}
