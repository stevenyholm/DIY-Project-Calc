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
using DiyProjectCalc.Infrastructure.Repositories;

namespace DiyProjectCalc.Tests.Integration.Controllers;

public class BasicShapesControllerTests : BaseDatabaseClassFixture
{
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _controller = new SUT.BasicShapesController(MapperHelper.CreateMapper(), 
            new EFBasicShapeRepository(base.DbContext),
            new EFProjectRepository(base.DbContext));
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Index(expectedProjectId);
        
        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShapeDTO>>().Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _controller.Details(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShapeId);
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
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

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
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _controller.Edit(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var editedModel = BasicShapeTestData.ValidBasicShape(base.DbContext);
        var editedModelId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);
        var editedModelDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            Id: editedModelId,
            ProjectId: editedModel?.ProjectId ?? -1
            );

        //Act
        var result = await _controller.Edit(editedModelId, editedModelDTO);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _controller.Delete(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var basicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _controller.DeleteConfirmed(basicShapeId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}
