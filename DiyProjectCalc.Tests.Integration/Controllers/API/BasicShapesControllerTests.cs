using SUT = DiyProjectCalc.Controllers.API;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Infrastructure.Data;
using System.Linq;

namespace DiyProjectCalc.Tests.Integration.Controllers.API;

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
    [Trait("GetAllForProject", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetAllForProject()
    {
        //Arrange
        var expectedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, expectedProject!.Id);

        //Act
        var result = await _controller.GetAllForProject(expectedProject!.Id);

        //Assert
        using (new AssertionScope())
        {
            result.As<ProjectDTOWithBasicShapes>().BasicShapes.Should().HaveCount(expectedCount);
            result.As<ProjectDTOWithBasicShapes>().Id.Should().Be(expectedProject.Id);
            result.As<ProjectDTOWithBasicShapes>().Name.Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.Get(project!.Id, expectedBasicShape!.Id);

        //Assert
        result.As<BasicShapeDTO>().Id.Should().Be(expectedBasicShape.Id);
    }

    [Fact]
    [Trait("Post", "POST")]
    public async Task ValidBasicShape_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShapeDTO = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

        //Act
        var result = await _controller.Post(projectId, newBasicShapeDTO);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put", "PUT")]
    public async Task ValidBasicShape_Returns_Ok_For_Put()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var basicShapeToEdit = project?.BasicShapes.First();
        var editedBasicShapeDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            Id: basicShapeToEdit!.Id,
            ProjectId: project!.Id
            );

        //Act
        var result = await _controller.Put(project.Id, editedBasicShapeDTO.Id, editedBasicShapeDTO);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete", "DELETE")]
    public async Task ValidBasicShapeId_Returns_ProjectId_For_Delete()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var deletedBasicShape = project?.BasicShapes.First();

        //Act
        var result = await _controller.Delete(project!.Id, deletedBasicShape!.Id);

        //Assert
        result.Should().BeOfType<OkResult>();
    }


}
