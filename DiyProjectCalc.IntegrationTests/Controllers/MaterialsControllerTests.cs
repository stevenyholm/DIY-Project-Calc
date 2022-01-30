using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.ViewModels;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.IntegrationTests.Controllers;

public class MaterialsControllerTests : BaseClassFixture
{
    public MaterialsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_Materials_For_Index_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFMaterialRepository(dbContext);
        var projectRepository = new EFProjectRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, projectRepository, null!);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await controller.Index(expectedProjectId);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<Material>>().Should().HaveCount(ProjectTestData.ValidProjectCountMaterials);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectTestData.ValidName);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Details_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFMaterialRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, null!);
        var expectedMaterialId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await controller.Details(expectedMaterialId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().MaterialId.Should().Be(expectedMaterialId);
    }

    [Fact]
    [Trait("Create", "GET")]
    public async Task ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFMaterialRepository(dbContext);
        var basicShapeRepository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, basicShapeRepository);
        var expectedProjectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await controller.Create(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Create_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFMaterialRepository(dbContext);
        var basicShapeRepository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, basicShapeRepository);
        var projectId = ProjectTestData.ValidProjectId(dbContext);
        var newMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = projectId,
            Material = MaterialTestData.NewMaterial
        };
        var newSelectedBasicShapeIds = dbContext.BasicShapes
            .Where(b => BasicShapeTestData.BasicShapeNamesForMaterialEdit.Any(n => n == b.Name))
            .Select(b => b.BasicShapeId).ToArray();

        //Act
        var result = await controller.Create(newMaterialEditViewModel, newSelectedBasicShapeIds);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Edit_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFMaterialRepository(dbContext);
        var basicShapeRepository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, basicShapeRepository);
        var expectedMaterialId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await controller.Edit(expectedMaterialId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().Material?.MaterialId.Should().Be(expectedMaterialId);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFMaterialRepository(dbContext);
        var basicShapeRepository = new EFBasicShapeRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, basicShapeRepository);
        var materialId = MaterialTestData.ValidMaterialId(dbContext);
        var projectId = ProjectTestData.ValidProjectId(dbContext);
        var editMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = projectId,
            Material = dbContext.Materials.FirstOrDefault(b => b.Name == MaterialTestData.ValidName)
        };
        if (editMaterialEditViewModel.Material is not null)
        {
            editMaterialEditViewModel.Material.Name = "2x4 redwood";
            editMaterialEditViewModel.Material.MeasurementType = MaterialMeasurement.Linear;
            editMaterialEditViewModel.Material.Length = 8.0;
            editMaterialEditViewModel.Material.Width = 3.5;
            editMaterialEditViewModel.Material.Depth = 1.5;
        }
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(dbContext);

        //Act
        var result = await controller.Edit(materialId, editMaterialEditViewModel, newSelectedBasicShapeIds);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Delete_Get()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new EFMaterialRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, null!);
        var expectedMaterialId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await controller.Delete(expectedMaterialId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().MaterialId.Should().Be(expectedMaterialId);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidMaterialId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new EFMaterialRepository(dbContext);
        var controller = new SUT.MaterialsController(repository, null!, null!);
        var materialId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await controller.DeleteConfirmed(materialId);
        dbContext.ChangeTracker.Clear();

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

