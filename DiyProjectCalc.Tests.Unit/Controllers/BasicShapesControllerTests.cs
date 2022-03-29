using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.SharedKernel.Interfaces;

namespace DiyProjectCalc.Tests.Unit.Controllers;

public class BasicShapesControllerTests 
{
    private Mock<IBasicShapeRepository> _mockRepository = new Mock<IBasicShapeRepository>();
    private Mock<IProjectRepository> _mockProjectRepository = new Mock<IProjectRepository>();
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests()
    {
        _controller = new SUT.BasicShapesController(MapperHelper.CreateMapper(), 
            _mockRepository.Object, 
            _mockProjectRepository.Object);
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithBasicShapes;
        _mockProjectRepository.Setup(r => r.GetProjectWithBasicShapesAsync(It.IsAny<int>())).ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Index(expectedProject.ProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShapeDTO>>().Should().HaveCount(expectedProject.BasicShapes.Count);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProject.ProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.Details(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = _controller.Create(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTO;

        //Act
        var result = await _controller.Create(newBasicShape);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Edit_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.Edit(expectedBasicShapeId); 

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var editedModel = BasicShapeTestData.MockSimpleBasicShapeDTO;
        var editedModelId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.Edit(editedModelId, editedModel);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var expectedBasicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.Delete(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var basicShape = BasicShapeTestData.MockSimpleBasicShape;
        _mockRepository.Setup(r => r.GetBasicShapeAsync(It.IsAny<int>())).ReturnsAsync(basicShape);
        var basicShapeId = BasicShapeTestData.MockSimpleBasicShapeId;

        //Act
        var result = await _controller.DeleteConfirmed(basicShapeId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

