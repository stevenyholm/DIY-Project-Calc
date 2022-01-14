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
public class BasicShapesControllerTests : ControllerTestsBase
{
    [Fact]
    public async Task Index_Get_ValidProjectId_ReturnsBasicShapes()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = await controller.Index(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShape>>().Should().HaveCount(ProjectsTestData.CountBasicShapesForProject);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectsTestData.NameValid);
        }
    }

    [Fact]
    public async Task Details_Get_ValidBasicShapeId_ReturnsBasicShape()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var projectId = base.ValidProjectId(dbContext);
            var expectedBasicShapeId = base.ValidBasicShapeId(dbContext);

            //Act
            var result = await controller.Details(projectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
        }
    }

    [Fact]
    public void Create_Get_ValidProjectId_ReturnsView()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = controller.Create(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);

        }
    }

    [Fact]
    public async Task Create_Post_ValidBasicShape_NoError()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var projectId = base.ValidProjectId(dbContext);
            var newBasicShape = BasicShapesTestData.NewBasicShape;
            newBasicShape.ProjectId = projectId;

            //Act
            var result = await controller.Create(newBasicShape);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    public async Task Edit_Get_ValidBasicShapeId_ReturnsBasicShape()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var expectedBasicShapeId = base.ValidBasicShapeId(dbContext);

            //Act
            var result = await controller.Edit(expectedBasicShapeId); 

            //Assert
            result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
        }
    }

    [Fact]
    public async Task Edit_Post_ValidBasicShape_NoError()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var editedModel = dbContext.BasicShapes.FirstOrDefault(b => b.Name == BasicShapesTestData.NameValid);
            var editedModelId = editedModel?.BasicShapeId ?? 0;
            if (editedModel is not null)
            {
                editedModel.Number1 = 55.0;
                editedModel.Number2 = 180.0;
                editedModel.ShapeType = BasicShapeType.Curved;
                editedModel.Name = "corner of door";
            }

            //Act
            var result = await controller.Edit(editedModelId, editedModel);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    public async Task Delete_Get_ValidBasicShapeId_ReturnsBasicShape()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var expectedBasicShapeId = base.ValidBasicShapeId(dbContext);

            //Act
            var result = await controller.Delete(expectedBasicShapeId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
        }
    }

    [Fact]
    public async Task Delete_Post_ValidBasicShapeId_NoError()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new BasicShapesController(dbContext);
            var basicShapeId = base.ValidBasicShapeId(dbContext);

            //Act
            var result = await controller.DeleteConfirmed(basicShapeId);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}

