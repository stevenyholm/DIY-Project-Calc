using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DiyProjectCalc.UnitTests.Controllers;

[Collection("Controllers")]
public class BasicShapesControllerTests 
{
    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        var project = new ProjectTestData().ValidProjectTestModel!.Project; 
        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);

        var mockRepository = new Mock<IBasicShapeRepository>();
        mockRepository.Setup(r => r.GetBasicShapesForProjectAsync(It.IsAny<int>())).ReturnsAsync(project.BasicShapes);

        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await controller.Index(expectedProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShape>>().Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;
        var mockRepository = new Mock<IBasicShapeRepository>();
        mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);

        //Act
        var result = await controller.Details(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        var mockRepository = new Mock<IBasicShapeRepository>();
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = controller.Create(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var mockRepository = new Mock<IBasicShapeRepository>();
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);
        var newBasicShape = BasicShapeTestData.NewBasicShape;
        var projectId = ProjectTestData.MockSimpleProjectId;
        newBasicShape.ProjectId = projectId;

        //Act
        var result = await controller.Create(newBasicShape);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Edit_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;
        var mockRepository = new Mock<IBasicShapeRepository>();
        mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);

        //Act
        var result = await controller.Edit(expectedBasicShapeId); 

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var editedModel = BasicShapeTestData.MockSimpleBasicShape;
        var editedModelId = BasicShapeTestData.MockSimpleBasicShapeId;
        var mockRepository = new Mock<IBasicShapeRepository>();
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);

        //Act
        var result = await controller.Edit(editedModelId, editedModel);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;
        var mockRepository = new Mock<IBasicShapeRepository>();
        mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);

        //Act
        var result = await controller.Delete(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        var mockRepository = new Mock<IBasicShapeRepository>();
        mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);

        var mockProjectRepository = new Mock<IProjectRepository>();
        var controller = new SUT.BasicShapesController(mockRepository.Object, mockProjectRepository.Object);
        var basicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await controller.DeleteConfirmed(basicShapeId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

