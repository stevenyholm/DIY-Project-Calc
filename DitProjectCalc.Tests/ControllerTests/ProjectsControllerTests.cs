using DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.ControllerTests.Abstractions;
using DiyProjectCalc.Tests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiyProjectCalc.Tests.ControllerTests;

[Collection("Controllers")]
public class ProjectsControllerTests : ControllerTestsBase
{
    [Fact]
    public void Index_Get_ReturnsAllProjects()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);

            //Act
            var result = controller.Index();

            //Assert
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<Project>>().Should().HaveCount(ProjectsTestData.CountAllProjects);
        }
    }

    [Fact]
    public async Task Details_Get_ValidProjectId_ReturnsProject()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = await controller.Details(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    public void Create_Get_ReturnsView()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);

            //Act
            var result = controller.Create();

            //Assert
            result.Should().BeOfType<ViewResult>();
        }
    }

    [Fact]
    public void Create_Post_ValidProject_NoError()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var project = ProjectsTestData.NewProject;

            //Act
            var result = controller.Create(project);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    public void Edit_Get_ValidProjectId_ReturnsProject()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = controller.Edit(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    public void Edit_Post_ValidProject_NoError()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var editedModel = dbContext.Projects.FirstOrDefault(b => b.Name == ProjectsTestData.NameValid);
            if (editedModel is not null)
            {
                editedModel.Name = "build a fence";
            }
            else
            {
                throw new System.Exception("Project not found");
            }

            //Act
            var result = controller.Edit(editedModel);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    public void Delete_Get_ValidProjectId_ReturnsProject()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = controller.Delete(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<Project>().ProjectId.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    public void Delete_Post_ValidProjectId_NoError() 
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new ProjectsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = controller.DeletePOST(expectedProjectId);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}

