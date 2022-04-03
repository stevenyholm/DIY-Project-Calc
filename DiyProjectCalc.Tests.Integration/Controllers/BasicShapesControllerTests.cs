using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Infrastructure.Data;
using System.Linq;

namespace DiyProjectCalc.Tests.Integration.Controllers;

public class BasicShapesControllerTests : BaseDatabaseClassFixture
{
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _controller = new SUT.BasicShapesController(
            MapperHelper.CreateMapper(), 
            new EfRepository<Project>(base.DbContext)
            );
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, expectedProject!.Id);

        //Act
        var result = await _controller.Index(expectedProject.Id);
        
        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShapeDTO>>().Should().HaveCount(expectedCount);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProject.Id);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.Details(project!.Id, expectedBasicShape!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

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
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShapeDTO = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

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
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.Edit(project!.Id, expectedBasicShape!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var editedBasicShape = project!.BasicShapes.First();
        var editedBasicShapeDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            Id: editedBasicShape.Id,
            ProjectId: editedBasicShape?.ProjectId ?? -1
            );

        //Act
        var result = await _controller.Edit(project.Id, editedBasicShape!.Id, editedBasicShapeDTO);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.Delete(project!.Id, expectedBasicShape!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var deletedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.DeleteConfirmed(project!.Id, deletedBasicShape!.Id);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}
