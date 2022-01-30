using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.IntegrationTests.Controllers;

public class ProjectsControllerTests : BaseClassFixture
{
    public ProjectsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("Index", "GET")]
    public async Task Returns_AllProjects_For_Index_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);

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
        using var dbContext = base.NewDbContext();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

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
        using var dbContext = base.NewDbContext();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);

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
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);
        var project = ProjectTestData.NewProject;

        //Act
        var result = await controller.Create(project);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Edit_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

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
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);

        var editedModel = ProjectTestData.ValidProject(dbContext);
        if (editedModel is not null)
        {
            editedModel.Name = "updated project";
        }

        //Act
        var result = controller.Edit(editedModel!);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Delete_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

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
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFProjectRepository(dbContext);
        var controller = new SUT.ProjectsController(repository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await controller.DeletePOST(expectedProjectId);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

