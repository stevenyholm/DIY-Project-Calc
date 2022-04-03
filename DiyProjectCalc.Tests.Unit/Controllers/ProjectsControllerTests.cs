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
using DiyProjectCalc.TestHelpers.UnitTestBase;

namespace DiyProjectCalc.Tests.Unit.Controllers;

public class ProjectsControllerTests : BaseControllerTests
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests()
    {
        _controller = new SUT.ProjectsController(
            MapperHelper.CreateMapper(), 
            base._mockProjectRepository.Object
            );
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task Returns_AllProjects_For_Index_Get()
    {
        //Arrange
        var projects = ProjectTestData.ProjectsFor(new ProjectTestData().ValidProjectTestModelList);
        base._mockProjectRepository.Setup(r => r.ListAsync(TestCancellationToken())).ReturnsAsync(projects);

        //Act
        var result = await _controller.Index();

        //Assert
        result.As<ViewResult>().ViewData.Model.As<List<ProjectDTO>>()
            .Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Details_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProject;
        base._mockProjectRepository.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Details(expectedProject.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().Id.Should().Be(expectedProject.Id);
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
        var newProjectDTO = ProjectTestData.NewProjectDTO;

        //Act
        var result = await _controller.Create(newProjectDTO);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Edit_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProject;
        base._mockProjectRepository.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Edit(expectedProject.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().Id.Should().Be(expectedProject.Id);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidProject_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var editedProjectDTO = ProjectTestData.MockSimpleProjectDTO;

        //Act
        var result = await _controller.Edit(editedProjectDTO!);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Delete_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProject;
        base._mockProjectRepository.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Delete(expectedProject.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<ProjectDTO>().Id.Should().Be(expectedProject.Id);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidProjectId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var deletedProject = ProjectTestData.MockSimpleProject;
        base._mockProjectRepository.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), TestCancellationToken()))
            .ReturnsAsync(deletedProject);

        //Act
        var result = await _controller.DeletePOST(deletedProject.Id);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

