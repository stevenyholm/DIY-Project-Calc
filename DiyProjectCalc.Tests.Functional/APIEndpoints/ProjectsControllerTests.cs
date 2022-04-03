using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Models.DTO;
using FluentAssertions.Execution;
using FluentAssertions;
using System.Net;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;

namespace DiyProjectCalc.Tests.Functional.APIEndpoints;

public class ProjectsControllerTests : BaseAPIEndpointClassFixture
{
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
    }

    [Fact]
    [Trait("Projects", "Route-GET")]
    public async Task GET_Projects()
    {
        //Arrange
        var expectedCount = ProjectTestData.ProjectsCount(base.DbContext);

        //Act
        var httpResponseMessage = await base.GetAsync($"projects");
        var result = await base.Deserialize<IEnumerable<ProjectDTO>>(httpResponseMessage);

        //Assert
        using (new AssertionScope())
        {
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.Should().HaveCount(expectedCount);
        }
    }

    [Fact]
    [Trait("Projects_ProjectId", "Route-GET")]
    public async Task GET_Projects_ProjectId()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var httpResponseMessage = await base.GetAsync($"projects/{expectedProjectId}");
        var result = await base.Deserialize<ProjectDTO>(httpResponseMessage);

        //Assert
        using (new AssertionScope())
        {
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            result.As<ProjectDTO>().Id.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    [Trait("Projects_with_Project", "Route-POST")]
    public async Task POST_Projects_with_Project()
    {
        //Arrange
        var newProjectDTO = ProjectTestData.NewProjectDTO;

        //Act
        var httpResponseMessage = await base.PostAsync($"projects", newProjectDTO);

        //Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);

    }

    [Fact]
    [Trait("Projects_with_Project", "Route-PUT")]
    public async Task PUT_Projects_with_Project()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var editedProjectDTO = new ProjectDTO(
            Id: projectId,
            Name: "roundy roundy"
            );

        //Act
        var httpResponseMessage = await base.PutAsync($"projects/{projectId}", editedProjectDTO);

        //Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Projects_ProjectId", "Route-DELETE")]
    public async Task DELETE_Projects_ProjectId()
    {
        //Arrange
        var deletedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var httpResponseMessage = await base.DeleteAsync($"projects/{deletedProjectId}");

        //Assert
        using (new AssertionScope())
        {
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
