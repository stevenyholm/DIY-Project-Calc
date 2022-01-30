using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.Repositories;
using Moq;

namespace DiyProjectCalc.UnitTests.Controllers;

public class ProjectsControllerTests 
{
    [Fact]
    [Trait("Index", "GET")]
    public async Task Returns_AllProjects_For_Index_Get()
    {
        //Arrange
        var projects = ProjectTestData.ProjectsFor(new ProjectTestData().ValidProjectTestModelList);
        var mockRepository = new Mock<IProjectRepository>();
        mockRepository.Setup(r => r.GetAllProjectsAsync()).ReturnsAsync(projects);
        var controller = new SUT.ProjectsController(mockRepository.Object);

        //Act
        var result = await controller.Index();

        //Assert
        result.As<ViewResult>().ViewData.Model.As<IEnumerable<Project>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var mockRepository = new Mock<IProjectRepository>();
        mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await controller.Details(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void Returns_View_For_Create_Get()
    {
        //Arrange
        var mockRepository = new Mock<IProjectRepository>();
        var controller = new SUT.ProjectsController(mockRepository.Object);

        //Act
        var result = controller.Create();

        //Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidProject_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var mockRepository = new Mock<IProjectRepository>();
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var project = ProjectTestData.NewProject;

        //Act
        var result = await controller.Create(project);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Edit_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var mockRepository = new Mock<IProjectRepository>();
        mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await controller.Edit(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public void ValidProject_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var mockRepository = new Mock<IProjectRepository>();
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var editedModel = ProjectTestData.MockSimpleProject;
        if (editedModel is not null)
        {
            editedModel.Name = "update project";
        }

        //Act
        var result = controller.Edit(editedModel!);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var mockRepository = new Mock<IProjectRepository>();
        mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await controller.Delete(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidProjectId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProject;
        var mockRepository = new Mock<IProjectRepository>();
        mockRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var controller = new SUT.ProjectsController(mockRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

        //Act
        var result = await controller.DeletePOST(expectedProjectId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

