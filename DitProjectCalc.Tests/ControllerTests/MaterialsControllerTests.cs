using DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.Tests.ControllerTests.Abstractions;
using DiyProjectCalc.Tests.TestData;
using DiyProjectCalc.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DiyProjectCalc.Tests.ControllerTests;

[Collection("Controllers")]
public class MaterialsControllerTests : ControllerTestsBase
{
    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_Materials_For_Index_Get()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = await controller.Index(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<Material>>().Should().HaveCount(ProjectsTestData.CountMaterialsForProject);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProjectId);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(ProjectsTestData.NameValid);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Details_Get()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var expectedMaterialId = base.ValidMaterialId(dbContext);

            //Act
            var result = await controller.Details(expectedMaterialId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<Material>().MaterialId.Should().Be(expectedMaterialId);
        }
    }

    [Fact]
    [Trait("Create", "GET")]
    public async Task ValidProjectId_Returns_View_For_Create_Get()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var expectedProjectId = base.ValidProjectId(dbContext);

            //Act
            var result = await controller.Create(expectedProjectId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().ProjectId.Should().Be(expectedProjectId);
        }
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Create_Post()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var projectId = base.ValidProjectId(dbContext);
            var newMaterialEditViewModel = new MaterialEditViewModel()
            {
                ProjectId = projectId,
                Material = MaterialsTestData.NewMaterial
            };
            var newSelectedBasicShapeIds = dbContext.BasicShapes
                .Where(b => BasicShapesTestData.BasicShapeNamesForMaterialEdit.Any(n => n == b.Name))
                .Select(b => b.BasicShapeId).ToArray();

            //Act
            var result = await controller.Create(newMaterialEditViewModel, newSelectedBasicShapeIds); 

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Edit_Get()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var expectedMaterialId = base.ValidMaterialId(dbContext);

            //Act
            var result = await controller.Edit(expectedMaterialId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().Material?.MaterialId.Should().Be(expectedMaterialId);
        }
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Edit_Post()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var materialId = base.ValidMaterialId(dbContext);
            var projectId = base.ValidProjectId(dbContext);
            var editMaterialEditViewModel = new MaterialEditViewModel()
            {
                ProjectId = projectId,
                Material = dbContext.Materials.FirstOrDefault(b => b.Name == MaterialsTestData.NameValid)
            };
            if (editMaterialEditViewModel.Material is not null)
            {
                editMaterialEditViewModel.Material.Name = "2x4 redwood";
                editMaterialEditViewModel.Material.MeasurementType = MaterialMeasurement.Linear;
                editMaterialEditViewModel.Material.Length = 8.0;
                editMaterialEditViewModel.Material.Width = 3.5;
                editMaterialEditViewModel.Material.Depth = 1.5;
            }
            var newSelectedBasicShapeIds = dbContext.BasicShapes
                .Where(b => BasicShapesTestData.BasicShapeNamesForMaterialEdit.Any(n => n == b.Name))
                .Select(b => b.BasicShapeId).ToArray();

            //Act
            var result = await controller.Edit(materialId, editMaterialEditViewModel, newSelectedBasicShapeIds);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Delete_Get()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var expectedMaterialId = base.ValidMaterialId(dbContext);

            //Act
            var result = await controller.Delete(expectedMaterialId);

            //Assert
            result.As<ViewResult>().ViewData.Model.As<Material>().MaterialId.Should().Be(expectedMaterialId);
        }
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidMaterialId_Throws_NoError_For_Edit_Post()
    {
        using (var dbContext = base.NewDbContext())
        {
            //Arrange
            var controller = new MaterialsController(dbContext);
            var materialId = base.ValidMaterialId(dbContext);

            //Act
            var result = await controller.DeleteConfirmed(materialId);

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();
        }
    }
}

