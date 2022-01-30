using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.IntegrationTests.Controllers;

public class BasicShapesControllerTests : BaseClassFixture
{
    public BasicShapesControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_BasicShapes_For_Index_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFBasicShapeRepository(dbContext);
        var projectRepository = new EFProjectRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, projectRepository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await controller.Index(expectedProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<BasicShape>>().Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Details_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await controller.Details(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public void ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = controller.Create(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Create_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var projectId = ProjectTestData.ValidProjectId(dbContext);
        var newBasicShape = BasicShapeTestData.NewBasicShape;
        newBasicShape.ProjectId = projectId;

        //Act
        var result = await controller.Create(newBasicShape);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Edit_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await controller.Edit(expectedBasicShapeId); 

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidBasicShape_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);

        var editedModel = BasicShapeTestData.ValidBasicShape(dbContext);
        var editedModelId = BasicShapeTestData.ValidBasicShapeId(dbContext);
        if (editedModel is not null)
        {
            editedModel.Number1 = 55.0;
            editedModel.Number2 = 180.0;
            editedModel.ShapeType = BasicShapeType.Curved;
            editedModel.Name = "corner of door";
        }

        //Act
        var result = await controller.Edit(editedModelId, editedModel!);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidBasicShapeId_Returns_BasicShape_For_Delete_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var expectedBasicShapeId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await controller.Delete(expectedBasicShapeId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<BasicShape>().BasicShapeId.Should().Be(expectedBasicShapeId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidBasicShapeId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.BasicShapesController(repository, null!);
        var basicShapeId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await controller.DeleteConfirmed(basicShapeId);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

