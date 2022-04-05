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
using DiyProjectCalc.TestHelpers.UnitTestBase;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using System.Linq;

namespace DiyProjectCalc.Tests.Unit.Controllers;

public class BasicShapesControllerTests : BaseControllerTests
{
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests()
    {
        _controller = new SUT.BasicShapesController(
            MapperHelper.CreateMapper(),
            base._mockProjectRepository.Object
            );
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithBasicShapes;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Index(expectedProject.Id);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShapeDTO>>()
                .Should().HaveCount(expectedProject.BasicShapes.Count());
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProject.Id);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Details(project.Id, expectedBasicShape.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
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
        var projectId = ProjectTestData.MockSimpleProjectWithBasicShapes.Id;
        var newBasicShapeDTO = BasicShapeTestData.NewBasicShapeDTO;

        //Act
        var result = await _controller.Create(projectId, newBasicShapeDTO);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Edit_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Edit(project.Id, expectedBasicShape.Id); 

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var projectId = ProjectTestData.MockSimpleProjectWithBasicShapes.Id;
        var editedBasicShapeDTO = BasicShapeTestData.MockSimpleBasicShapeDTO;

        //Act
        var result = await _controller.Edit(projectId, editedBasicShapeDTO.Id, editedBasicShapeDTO);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var expectedBasicShape = project.BasicShapes.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Delete(project.Id, expectedBasicShape.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithBasicShapes;
        var deletedBasicShape = project.BasicShapes.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithBasicShapesSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.DeleteConfirmed(project.Id, deletedBasicShape.Id);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

