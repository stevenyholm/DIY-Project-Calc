using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Models.DTO;

namespace DiyProjectCalc.Tests.Integration.Controllers;

public class ProjectsControllerTests : BaseDatabaseClassFixture
{
    private SUT.ProjectsController _controller;
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _controller = new SUT.ProjectsController(MapperHelper.CreateMapper(), new EFProjectRepository(base.DbContext));
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task Returns_AllProjects_For_Index_Get()
    {
        //Arrange

        //Act
        var result = await _controller.Index();

        //Assert
        result.As<ViewResult>().ViewData.Model.As<IEnumerable<ProjectDTO>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Details_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

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
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

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
        var editedModel = ProjectTestData.ValidProjectDTO(base.DbContext);

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
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

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
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.DeletePOST(expectedProjectId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

