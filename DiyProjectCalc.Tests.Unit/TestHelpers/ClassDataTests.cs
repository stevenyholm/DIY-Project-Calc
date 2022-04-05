using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestModels;

namespace DiyProjectCalc.Tests.Unit.TestHelpers;
public class ClassDataTests
{
    [Fact]
    [Trait("MaterialValidClassData", "")]
    public void MaterialValidClassData_contains_correct_data()
    {
        //Arrange
        var enumerable = new MaterialValidClassData();
        var result = new List<MaterialTestModel>();

        //Act
        foreach(var objectArray in enumerable)
        {
            foreach(var item in objectArray)
            {
                var model = (MaterialTestModel)item;
                result.Add(model);
            }
        }

        //Assert
        result.Should().HaveCountGreaterThan(0);
        foreach(var resultModel in result)
        {
            resultModel.Material.Should().NotBeNull();
            resultModel.Material.BasicShapes.Should().HaveCountGreaterThan(0);
        }
    }

    [Fact]
    [Trait("BasicShapeValidClassData", "")]
    public void BasicShapeValidClassData_contains_correct_data()
    {
        //Arrange
        var enumerable = new BasicShapeValidClassData();
        var result = new List<BasicShapeTestModel>();

        //Act
        foreach (var objectArray in enumerable)
        {
            foreach (var item in objectArray)
            {
                var model = (BasicShapeTestModel)item;
                result.Add(model);
            }
        }

        //Assert
        result.Should().HaveCountGreaterThan(0);
        foreach (var resultModel in result)
        {
            resultModel.BasicShape.Should().NotBeNull();
        }
    }

    [Fact]
    [Trait("ProjectValidClassData", "")]
    public void ProjectValidClassData_contains_correct_data()
    {
        //Arrange
        var enumerable = new ProjectValidClassData();
        var result = new List<ProjectTestModel>();

        //Act
        foreach (var objectArray in enumerable)
        {
            foreach (var item in objectArray)
            {
                var model = (ProjectTestModel)item;
                result.Add(model);
            }
        }

        //Assert
        result.Should().HaveCountGreaterThan(0);
        foreach (var resultModel in result)
        {
            resultModel.Project.Should().NotBeNull();
        }
    }

}
