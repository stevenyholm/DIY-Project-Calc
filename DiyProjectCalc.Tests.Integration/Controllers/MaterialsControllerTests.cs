using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.ViewModels;
using DiyProjectCalc.TestHelpers.TestData;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Infrastructure.Data;
using System.Linq;

namespace DiyProjectCalc.Tests.Integration.Controllers;

public class MaterialsControllerTests : BaseDatabaseClassFixture
{
    private SUT.MaterialsController _controller;
    public MaterialsControllerTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _controller = new SUT.MaterialsController(
            MapperHelper.CreateMapper(),
            new EfRepository<Project>(base.DbContext)
            );
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_Materials_For_Index_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, expectedProject!.Id);

        //Act
        var result = await _controller.Index(expectedProject.Id);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<Material>>().Should().HaveCount(expectedCount);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProject.Id);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedMaterial = project?.Materials.First();

        //Act
        var result = await _controller.Details(project!.Id, expectedMaterial!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Create", "GET")]
    public async Task ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _controller.Create(expectedProjectId);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().ProjectId.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = projectId,
            Material = MaterialTestData.NewMaterial
        };
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(base.DbContext);

        //Act
        var result = await _controller.Create(projectId, newMaterialEditViewModel, newSelectedBasicShapeIds);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Edit_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedMaterial = project?.Materials.First();

        //Act
        var result = await _controller.Edit(project!.Id, expectedMaterial!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().Material?.Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var editedMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = project!.Id,
            Material = project!.Materials.First()
        };
        if (editedMaterialEditViewModel.Material is not null)
        {
            editedMaterialEditViewModel.Material.Name = "2x4 redwood";
            editedMaterialEditViewModel.Material.MeasurementType = MaterialMeasurement.Linear;
            editedMaterialEditViewModel.Material.Length = 8.0;
            editedMaterialEditViewModel.Material.Width = 3.5;
            editedMaterialEditViewModel.Material.Depth = 1.5;
        }
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(base.DbContext);

        //Act
        var result = await _controller.Edit(
            project!.Id, 
            editedMaterialEditViewModel!.Material!.Id, 
            editedMaterialEditViewModel, 
            newSelectedBasicShapeIds
            );

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var expectedMaterial = project?.Materials.First();

        //Act
        var result = await _controller.Delete(project!.Id, expectedMaterial!.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidMaterialId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.ValidProject(base.DbContext);
        var deletedMaterial = project?.Materials.First();

        //Act
        var result = await _controller.DeleteConfirmed(project!.Id, deletedMaterial!.Id);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}