using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestFixtures;
using System.Net;
using FluentAssertions.Execution;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using System.Linq;

namespace DiyProjectCalc.Tests.Functional.APIEndpoints;

public class BasicShapesControllerTests : BaseAPIEndpointClassFixture
{
    public BasicShapesControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes", "Route-GET")]
    public async Task GET_Projects_ProjectId_BasicShapes()
    {
        //Arrange
        var expectedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, expectedProject!.Id);

        //Act
        var httpResponseMessage = await base.GetAsync($"projects/{expectedProject.Id}/basicshapes");
        var result = await base.Deserialize<ProjectDTOWithBasicShapes>(httpResponseMessage);

        //Assert
        using (new AssertionScope())
        {
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.BasicShapes.Should().HaveCount(expectedCount);
            result?.Id.Should().Be(expectedProject.Id);
            result?.Name.Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes_BasicShapeId", "Route-GET")]
    public async Task GET_Projects_ProjectId_BasicShapes_BasicShapeId()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var httpResponseMessage = await base.GetAsync($"projects/{project!.Id}/basicshapes/{expectedBasicShape!.Id}");
        var result = await base.Deserialize<BasicShapeDTO>(httpResponseMessage);

        //Assert
        using (new AssertionScope())
        {
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.Id.Should().Be(expectedBasicShape.Id);
        }
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes_with_BasicShape", "Route-POST")]
    public async Task POST_Projects_ProjectId_BasicShapes_with_BasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newBasicShapeDTO = BasicShapeTestData.NewBasicShapeDTO;

        //Act
        var httpResponseMessage = await base.PostAsync($"projects/{projectId}/basicshapes", newBasicShapeDTO);

        //Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes_with_BasicShape", "Route-PUT")]
    public async Task PUT_Projects_ProjectId_BasicShapes_with_BasicShape()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var basicShapeToUpdate = project?.BasicShapes.First();
        var editedBasicShapeDTO = new BasicShapeDTO(
            ShapeType: BasicShapeType.Curved,
            Name: "corner of door",
            Number1: 55.0,
            Number2: 100.0,
            Id: basicShapeToUpdate!.Id,
            ProjectId: project!.Id
            );

        //Act
        var httpResponseMessage = await base.PutAsync($"projects/{project.Id}/basicshapes/{editedBasicShapeDTO.Id}", editedBasicShapeDTO);

        //Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Projects_ProjectId_BasicShapes_BasicShapeId", "Route-DELETE")]
    public async Task DELETE_Projects_ProjectId_BasicShapes_BasicShapeId()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var deletedBasicShape = project?.BasicShapes.First();

        //Act
        var httpResponseMessage = await base.DeleteAsync($"projects/{project!.Id}/basicshapes/{deletedBasicShape!.Id}");

        //Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
