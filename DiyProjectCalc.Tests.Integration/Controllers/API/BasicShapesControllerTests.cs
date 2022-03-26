using SUT = DiyProjectCalc.Controllers.API;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Integration.Controllers.API;

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
    [Trait("GetAllForProject", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetAllForProject()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.GetAllForProject(expectedProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ProjectDTOWithBasicShapes>().BasicShapes.Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result.As<ProjectDTOWithBasicShapes>().ProjectId.Should().Be(expectedProjectId);
            result.As<ProjectDTOWithBasicShapes>().Name.Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Get()
    {
        //Arrange
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _controller.Get(expectedBasicShapeId);

        //Assert
        result.As<BasicShapeDTO>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Post", "POST")]
    public async Task ValidBasicShape_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

        //Act
        var result = await _controller.Post(newBasicShape);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put", "PUT")]
    public async Task ValidBasicShape_Returns_Ok_For_Put()
    {
        //Arrange
        var editedModel = BasicShapeTestData.ValidBasicShape(base.DbContext);
        var editedModelId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);
        var editedModelDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            BasicShapeId: editedModelId,
            ProjectId: editedModel?.ProjectId ?? -1
            );

        //Act
        var result = await _controller.Put(editedModelId, editedModelDTO);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete", "DELETE")]
    public async Task ValidBasicShapeId_Returns_ProjectId_For_Delete()
    {
        //Arrange
        var basicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Delete(basicShapeId);

        //Assert
        result.As<ActionResult<int>>().Value.Should().Be(projectId);
    }


}
