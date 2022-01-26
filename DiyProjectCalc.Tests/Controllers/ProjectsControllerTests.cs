using DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.Controllers.Abstractions;
using DiyProjectCalc.Tests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiyProjectCalc.Tests.Controllers;

[Collection("Controllers")]
public class ProjectsControllerTests : ControllerTestsBase
{
    [Fact]
    [Trait("Index", "GET")]
    public void Returns_AllProjects_For_Index_Get()
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
    [Trait("Details", "GET")]
    public async Task ValidProjectId_Returns_Project_For_Details_Get()
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
    [Trait("Create", "GET")]
    public void Returns_View_For_Create_Get()
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
    [Trait("Create", "POST")]
    public void ValidProject_Throws_NoError_For_Create_Post()
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
    [Trait("Edit", "GET")]
    public void ValidProjectId_Returns_Project_For_Edit_Get()
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
    [Trait("Edit", "POST")]
    public void ValidProject_Throws_NoError_For_Edit_Post()
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
    [Trait("Delete", "GET")]
    public void ValidProjectId_Returns_Project_For_Delete_Get()
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
    [Trait("Delete", "POST")]
    public void ValidProjectId_Throws_NoError_For_Delete_Post()
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

