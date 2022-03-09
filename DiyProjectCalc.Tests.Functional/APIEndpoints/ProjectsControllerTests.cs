using SUT = DiyProjectCalc.Controllers.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.Models.DTO;
using FluentAssertions.Execution;
using FluentAssertions;
using System.Net;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;

namespace DiyProjectCalc.Tests.Functional.APIEndpoints;

public class ProjectsControllerTests : BaseAPIEndpointClassFixture
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(),
            new EFProjectRepository(base.DbContext));
    }

    [Fact]
    [Trait("Projects", "Route-GET")]
    public async Task GET_Projects()
    {
        //Arrange

        //Act
        var response = await base.GetAsync($"projects");
        var result = await base.Deserialize<IEnumerable<ProjectDTO>>(response);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result?.Should().HaveCount(ProjectTestData.ValidProjectListCount);
        }
    }

    [Fact]
    [Trait("Projects_ProjectId", "Route-GET")]
    public async Task GET_Projects_ProjectId()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var response = await base.GetAsync($"projects/{expectedProjectId}");
        var result = await base.Deserialize<ProjectDTO>(response);

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.As<ProjectDTO>().ProjectId.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    [Trait("Projects_with_Project", "Route-POST")]
    public async Task POST_Projects_with_Project()
    {
        //Arrange
        var newModel = ProjectTestData.NewProjectDTO;

        //Act
        var response = await base.PostAsync($"projects", newModel);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

    }

    [Fact]
    [Trait("Projects_with_Project", "Route-PUT")]
    public async Task PUT_Projects_with_Project()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var editedModelDTO = new ProjectDTO(
            ProjectId: projectId,
            Name: "roundy roundy"
            );

        //Act
        var response = await base.PutAsync($"projects/{projectId}", editedModelDTO);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    [Trait("Projects_ProjectId", "Route-DELETE")]
    public async Task DELETE_Projects_ProjectId()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var response = await base.DeleteAsync($"projects/{expectedProjectId}");

        //Assert
        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
