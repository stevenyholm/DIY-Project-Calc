using SUT = DiyProjectCalc.Controllers.API;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestFixtures;
using System.Net;
using DiyProjectCalc.TestHelpers.Helpers;
using FluentAssertions.Execution;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Functional.APIEndpoints;

public class BasicShapesControllerTests : BaseAPIEndpointClassFixture
{
    private SUT.BasicShapesController _controller;
    public BasicShapesControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _controller = new SUT.BasicShapesController(MapperHelper.CreateMapper(), 
            new EFBasicShapeRepository(base.DbContext),
            new EFProjectRepository(base.DbContext));
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes", "Route-GET")]
    public async Task GET_Projects_ProjectId_BasicShapes()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var response = await base.GetAsync($"projects/{expectedProjectId}/basicshapes");
        var result = await base.Deserialize<ProjectDTOWithBasicShapes>(response);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.BasicShapes.Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result?.ProjectId.Should().Be(expectedProjectId);
            result?.Name.Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("BasicShapes_BasicShapeId", "Route-GET")]
    public async Task GET_BasicShapes_BasicShapeId()
    {
        //Arrange
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var response = await base.GetAsync($"basicshapes/{expectedBasicShapeId}");
        var result = await base.Deserialize<BasicShapeDTO>(response);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.BasicShapeId.Should().Be(expectedBasicShapeId);
        }
    }

    [Fact]
    [Trait("BasicShapes_with_BasicShape", "Route-POST")]
    public async Task POST_BasicShapes_with_BasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);

        //Act
        var response = await base.PostAsync($"basicshapes", newBasicShape);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("BasicShapes_with_BasicShape", "Route-PUT")]
    public async Task PUT_BasicShapes_with_BasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShape = BasicShapeTestData.NewBasicShapeDTOWithProjectId(projectId);
        var editedModelDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            BasicShapeId: newBasicShape.BasicShapeId,
            ProjectId: projectId
            );

        //Act
        var response = await base.PutAsync($"basicshapes/{newBasicShape.BasicShapeId}", editedModelDTO);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("BasicShapes_BasicShapeId", "Route-DELETE")]
    public async Task DELETE_BasicShapes_BasicShapeId()
    {
        //Arrange
        var basicShapeId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var response = await base.DeleteAsync($"basicshapes/{basicShapeId}");
        var result = await base.Deserialize<int>(response);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().Be(projectId);
        }
    }
}
