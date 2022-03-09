using SUT = DiyProjectCalc.Controllers.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.TestHelpers.Helpers;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestData;

namespace DiyProjectCalc.Tests.Integration.Controllers.API;
public class ProjectsControllerTests : BaseDatabaseClassFixture
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(),
            new EFProjectRepository(base.DbContext));
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task Returns_AllProjects_For_Get()
    {
        //Arrange

        //Act
        var result = await _controller.Get();

        //Assert
        result.As<IEnumerable<ProjectDTO>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("Get_ProjectId", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Get(expectedProjectId);

        //Assert
        result.As<ProjectDTO>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Post_Project", "POST")]
    public async Task ValidProject_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var newModel = ProjectTestData.NewProjectDTO;

        //Act
        var result = await _controller.Post(newModel);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put_Project", "PUT")]
    public async Task ValidProject_Returns_Ok_For_Put()
    {
        //Arrange
        var editedModel = ProjectTestData.ValidProjectDTO(base.DbContext);

        //Act
        var result = await _controller.Put(editedModel?.ProjectId ?? -1, editedModel!);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_ProjectId", "DELETE")]
    public async Task ValidProjectId_Returns_Ok_For_Delete()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Delete(expectedProjectId);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

}
