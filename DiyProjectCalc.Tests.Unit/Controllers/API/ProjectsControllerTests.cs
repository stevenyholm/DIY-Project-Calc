using DiyProjectCalc.TestHelpers.Helpers;
using Moq;
using SUT = DiyProjectCalc.Controllers.API;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Interfaces;

namespace DiyProjectCalc.Tests.Unit.Controllers.API;
public class ProjectsControllerTests
{
    private Mock<IProjectRepository> _mockRepository = new Mock<IProjectRepository>();
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests()
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(), _mockRepository.Object);
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task Returns_AllProjects_For_Get()
    {
        //Arrange
        var projects = ProjectTestData.ProjectsFor(new ProjectTestData().ValidProjectTestModelList);
        _mockRepository.Setup(r => r.GetAllProjectsAsync()).ReturnsAsync(projects);

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
        var expectedProject = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Get(expectedProject.Id);

        //Assert
        result.As<ProjectDTO>().Id.Should().Be(expectedProject.Id);
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
        var editedModel = ProjectTestData.MockSimpleProjectDTO;

        //Act
        var result = await _controller.Put(editedModel.Id, editedModel);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_ProjectId", "DELETE")]
    public async Task ValidProjectId_Returns_Ok_For_Delete()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var projectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await _controller.Delete(projectId);

        //Assert
        result.Should().BeOfType<OkResult>();
    }


}
