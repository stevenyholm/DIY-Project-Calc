﻿using DiyProjectCalc.Models;
using DiyProjectCalc.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DiyProjectCalc.Tests.ViewModelTests;

public class MaterialEditViewModelTests
{
    [Fact]
    [Trait("Material.ProjectId", "")]
    public void SameObject_Given_ValidProjectId_For_MaterialProperty()
    {
        //Arrange
        var materialEditViewModel = new MaterialEditViewModel();
        materialEditViewModel.Material = new Material()
        {
            ProjectId = 5,
            MeasurementType = MaterialMeasurement.Linear,
            Length = 5.0,
            Name = "gravel",
            BasicShapes = new HashSet<BasicShape>()
        };

        //Act
        var result = materialEditViewModel.Material.ProjectId;

        //Assert
        result.Should().Be(5);
    }

    [Fact]
    [Trait("Material.ProjectId", "")]
    public void CorrectProjectId_Given_InvalidProjectId_For_MaterialProperty()
    {
        //Arrange
        var materialEditViewModel = new MaterialEditViewModel();
        materialEditViewModel.ProjectId = 5;
        materialEditViewModel.Material = new Material()
        {
            ProjectId = 0,
            MeasurementType = MaterialMeasurement.Linear,
            Length = 5.0,
            Name = "gravel",
            BasicShapes = new HashSet<BasicShape>()
        };

        //Act
        var result = materialEditViewModel.Material.ProjectId;

        //Assert
        result.Should().Be(5);
    }

    [Fact]
    [Trait("BasicShapesData", "")]
    public void CorrectNumberSelected_Given_ValidState_For_BasicShapesData()
    {
        //Arrange
        var materialEditViewModel = new MaterialEditViewModel();
        materialEditViewModel.ProjectId = 5;
        materialEditViewModel.Material = new Material()
        {
            ProjectId = 0,
            MeasurementType = MaterialMeasurement.Linear,
            Length = 5.0,
            Name = "gravel",
            BasicShapes = new HashSet<BasicShape>()
            {
                new BasicShape() { BasicShapeId = 1, ShapeType = BasicShapeType.Rectangle, Number1 = 5.0, Number2 = 2.0 }, 
                new BasicShape() { BasicShapeId = 4, ShapeType = BasicShapeType.Triangle, Number1 = 15.0, Number2 = 12.0 }
            }
        };
        materialEditViewModel.BasicShapesForProject = new List<BasicShape>()
        {
                new BasicShape() { BasicShapeId = 1, ShapeType = BasicShapeType.Rectangle, Number1 = 5.0, Number2 = 2.0 },
                new BasicShape() { BasicShapeId = 2, ShapeType = BasicShapeType.Curved, Number1 = 5.0, Number2 = 2.0 },
                new BasicShape() { BasicShapeId = 3, ShapeType = BasicShapeType.Rectangle, Number1 = 50.0, Number2 = 20.0 },
                new BasicShape() { BasicShapeId = 4, ShapeType = BasicShapeType.Triangle, Number1 = 15.0, Number2 = 12.0 }
        };

        //Act
        var result = materialEditViewModel.BasicShapesData();

        //Assert
        result.FindAll(b => b.Selected == true).Should().HaveCount(2);
    }
}
