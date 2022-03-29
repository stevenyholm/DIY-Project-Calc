using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Models.DTO;
using DiyProjectCalc.Core.Interfaces;

namespace DiyProjectCalc.Tests.Unit.Controllers;

public class ProjectsControllerTests 
{
    private Mock<IProjectRepository> _mockRepository = new Mock<IProjectRepository>();
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests()
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(), _mockRepository.Object);
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task Returns_AllProjects_For_Index_Get()
    {
        //Arrange
        var projects = ProjectTestData.ProjectsFor(new ProjectTestData().ValidProjectTestModelList);
        _mockRepository.Setup(r => r.GetAllProjectsAsync()).ReturnsAsync(projects);

        //Act
        var result = await _controller.Index();

        //Assert
        result.As<ViewResult>().ViewData.Model.As<List<ProjectDTO>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await _controller.Details(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void Returns_View_For_Create_Get()
    {
        //Arrange

        //Act
        var result = _controller.Create();

        //Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidProject_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var project = ProjectTestData.NewProjectDTO;

        //Act
        var result = await _controller.Create(project);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Edit_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await _controller.Edit(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidProject_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var editedModel = ProjectTestData.MockSimpleProjectDTO;

        //Act
        var result = await _controller.Edit(editedModel!);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await _controller.Delete(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidProjectId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        _mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await _controller.DeletePOST(expectedProjectId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

