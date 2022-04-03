using SUT = DiyProjectCalc.Controllers.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Integration.Controllers.API;
public class ProjectsControllerTests : BaseDatabaseClassFixture
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _controller = new SUT.ProjectsController(
            MapperHelper.CreateMapper(),
            new EfRepository<Project>(base.DbContext) 
            );
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task Returns_AllProjects_For_Get()
    {
        //Arrange
        var expectedCount = ProjectTestData.ProjectsCount(base.DbContext);

        //Act
        var result = await _controller.Get();

        //Assert
        result.As<IEnumerable<ProjectDTO>>().Should().HaveCount(expectedCount);
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
        result.As<ProjectDTO>().Id.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Post_Project", "POST")]
    public async Task ValidProject_Returns_CreatedAtActionResult_For_Post()
    {
        //Arrange
        var newProjectDTO = ProjectTestData.NewProjectDTO;

        //Act
        var result = await _controller.Post(newProjectDTO);

        //Assert
        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    [Trait("Put_Project", "PUT")]
    public async Task ValidProject_Returns_Ok_For_Put()
    {
        //Arrange
        var editedProjectDTO = ProjectTestData.ValidProjectDTO(base.DbContext);

        //Act
        var result = await _controller.Put(editedProjectDTO?.Id ?? -1, editedProjectDTO!);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_ProjectId", "DELETE")]
    public async Task ValidProjectId_Returns_Ok_For_Delete()
    {
        //Arrange
        var deletedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Delete(deletedProjectId);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

}
