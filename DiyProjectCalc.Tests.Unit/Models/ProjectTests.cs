using System.Linq;
using FluentAssertions;
using Xunit;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.TestHelpers.TestData;

namespace DiyProjectCalc.Tests.Unit.Models;
public class ProjectTests
{
    //======================= BasicShapes Aggregate ================================

    [Fact]
    [Trait("GetBasicShape", "")]
    public void ValidInput_Gets_Item_For_GetBasicShape()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();

        //Act
        var result = project.GetBasicShape(expectedBasicShape.Id);

        //Assert
        result.As<BasicShape>().Name.Should().Be(expectedBasicShape.Name);
    }

    [Fact]
    [Trait("BasicShapeExists", "")]
    public void ValidInput_Returns_True_For_BasicShapeExists()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();

        //Act
        var result = project.BasicShapeExists(expectedBasicShape.Id);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    [Trait("AddBasicShape", "")]
    public void ValidInput_Adds_Item_For_AddBasicShape()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var expectedCount = project.BasicShapes.Count() + 1;
        var newBasicShape = BasicShapeTestData.NewBasicShape;

        //Act
        project.AddBasicShape(newBasicShape);

        //Assert
        var afterCount = project.BasicShapes.Count();
        afterCount.Should().Be(expectedCount);
    }

    [Fact]
    [Trait("UpdateBasicShape", "")]
    public void ValidInput_Updates_Item_For_UpdateBasicShape()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var basicShape = project.BasicShapes.First();
        var expectedName = "edited basic shape";
        var editedBasicShape = new BasicShape()
        {
            Id = basicShape.Id,
            Name = expectedName,
            ShapeType = BasicShapeType.Triangle,
            Number1 = 100.1,
            Number2 = 200.2
        };

        //Act
        project.UpdateBasicShape(editedBasicShape!, MapperHelper.CreateMapper());

        //Assert
        var result = project.BasicShapes.First(b => b.Id == basicShape.Id);
        result.As<BasicShape>().Name.Should().Be(expectedName);
    }

    [Fact]
    [Trait("RemoveBasicShape", "")]
    public void ValidInput_Removes_Item_For_RemoveBasicShape()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedCount = project.BasicShapes.Count() - 1;
        var removedBasicShape = project.BasicShapes.First();

        //Act
        project.RemoveBasicShape(removedBasicShape.Id);

        //Assert
        var afterCount = project.BasicShapes.Count();
        afterCount.Should().Be(expectedCount);
    }


    //======================= Materials Aggregate ================================

    [Fact]
    [Trait("GetMaterial", "")]
    public void ValidInput_Gets_Item_For_GetMaterial()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var expectedMaterial = project.Materials.First();

        //Act
        var result = project.GetMaterial(expectedMaterial.Id);

        //Assert
        result.As<Material>().Name.Should().Be(expectedMaterial.Name);
    }

    [Fact]
    [Trait("MaterialExists", "")]
    public void ValidInput_Returns_True_For_MaterialExists()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var expectedMaterial = project.Materials.First();

        //Act
        var result = project.MaterialExists(expectedMaterial.Id);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    [Trait("AddMaterial", "")]
    public void ValidInput_Adds_Item_For_AddMaterial()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var expectedCount = project.Materials.Count() + 1;
        var newMaterial = MaterialTestData.NewMaterial;

        //Act
        project.AddMaterial(newMaterial);

        //Assert
        var afterCount = project.Materials.Count();
        afterCount.Should().Be(expectedCount);
    }

    [Fact]
    [Trait("UpdateMaterial", "")]
    public void ValidInput_Updates_Item_For_UpdateMaterial()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var material = project.Materials.First();
        var expectedName = "edited material";
        var editedMaterial = new Material()
        {
            Id = material.Id,
            Name = expectedName,
            MeasurementType = MaterialMeasurement.Area,
            Length = 99.1,
            Depth = 88.1,
            Width = 77.1
        };

        //Act
        project.UpdateMaterial(editedMaterial!, MapperHelper.CreateMapper());

        //Assert
        var result = project.Materials.First(m => m.Id == material.Id);
        result.As<Material>().Name.Should().Be(expectedName);
    }

    [Fact]
    [Trait("RemoveMaterial", "")]
    public void ValidInput_Removes_Item_For_RemoveMaterial()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var expectedCount = project.Materials.Count() - 1;
        var removedMaterial = project.Materials.First();

        //Act
        project.RemoveMaterial(removedMaterial.Id);

        //Assert
        var afterCount = project.Materials.Count();
        afterCount.Should().Be(expectedCount);
    }

}
