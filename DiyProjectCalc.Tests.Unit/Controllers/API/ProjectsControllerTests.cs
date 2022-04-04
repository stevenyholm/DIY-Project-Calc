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
using DiyProjectCalc.TestHelpers.UnitTestBase;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;

namespace DiyProjectCalc.Tests.Unit.Controllers.API;
public class ProjectsControllerTests : BaseControllerTests
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests()
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(), 
            base._mockProjectRepository.Object);
    }

    [Fact]
    [Trait("Get", "GET")]
    public async Task Returns_AllProjects_For_Get()
    {
        //Arrange
        var projects = ProjectTestData.ProjectsFor(new ProjectTestData().ValidProjectTestModelList);
        base._mockProjectRepository.Setup(r => r.ListAsync(TestCancellationToken())).ReturnsAsync(projects);

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
        base._mockProjectRepository.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

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
        var newProjectDTO = ProjectTestData.NewProjectDTO;
        base._mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>(), TestCancellationToken()))
            .ReturnsAsync(new Project());

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
        var editedProjectDTO = ProjectTestData.MockSimpleProjectDTO;

        //Act
        var result = await _controller.Put(editedProjectDTO.Id, editedProjectDTO);

        //Assert
        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    [Trait("Delete_ProjectId", "DELETE")]
    public async Task ValidProjectId_Returns_Ok_For_Delete()
    {
        //Arrange
        var deletedProject = ProjectTestData.MockSimpleProject;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithAggregatesSpec>(), TestCancellationToken()))
            .ReturnsAsync(deletedProject);

        //Act
        var result = await _controller.Delete(deletedProject.Id);

        //Assert
        result.Should().BeOfType<OkResult>();
    }


}
