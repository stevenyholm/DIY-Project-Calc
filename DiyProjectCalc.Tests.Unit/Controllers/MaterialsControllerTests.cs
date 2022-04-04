using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.ViewModels;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.TestHelpers.UnitTestBase;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using System.Linq;

namespace DiyProjectCalc.Tests.Unit.Controllers;

public class MaterialsControllerTests : BaseControllerTests
{
    private SUT.MaterialsController _controller;
    public MaterialsControllerTests()
    {
        _controller = new SUT.MaterialsController(
            MapperHelper.CreateMapper(),
            base._mockProjectRepository.Object
            );
    }

    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_Materials_For_Index_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithMaterials;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Index(expectedProject.Id);

        //Assert
        using (new AssertionScope())
        {
            result.As<ViewResult>().ViewData.Model.As<IEnumerable<Material>>()
                .Should().HaveCount(expectedProject.Materials.Count);
            result.As<ViewResult>().ViewData["ProjectId"].Should().Be(expectedProject.Id);
            result.As<ViewResult>().ViewData["ProjectName"].Should().Be(expectedProject.Name);
        }
    }

    [Fact]
    [Trait("Details", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Details_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var expectedMaterial = project.Materials.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Details(project.Id, expectedMaterial.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Create", "GET")]
    public async Task ValidProjectId_Returns_View_For_Create_Get()
    {
        //Arrange
        var expectedProject = ProjectTestData.MockSimpleProjectWithMaterials;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(expectedProject);

        //Act
        var result = await _controller.Create(expectedProject.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().ProjectId.Should().Be(expectedProject.Id);
    }

    [Fact]
    [Trait("Create", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Create_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);
        var newMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = project.Id,
            Material = MaterialTestData.NewMaterial
        };
        var newSelectedBasicShapeIds = MaterialTestData.MockBasicShapeIdsForMaterialEdit;


        //Act
        var result = await _controller.Create(project.Id, newMaterialEditViewModel, newSelectedBasicShapeIds); 

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Edit_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);
        var expectedMaterial = project.Materials.First();

        //Act
        var result = await _controller.Edit(project.Id, expectedMaterial.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<MaterialEditViewModel>().Material?.Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Edit", "POST")]
    public async Task ValidMaterial_Throws_NoError_For_Edit_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var materialToUpdate = project.Materials.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);
        var editedMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = project.Id,
            Material = MaterialTestData.MockMaterialWithBasicShapes 
        };
        if (editedMaterialEditViewModel.Material is not null)
        {
            editedMaterialEditViewModel.Material.Id = materialToUpdate.Id;
            editedMaterialEditViewModel.Material.Name = "2x4 redwood";
            editedMaterialEditViewModel.Material.MeasurementType = MaterialMeasurement.Linear;
            editedMaterialEditViewModel.Material.Length = 8.0;
            editedMaterialEditViewModel.Material.Width = 3.5;
            editedMaterialEditViewModel.Material.Depth = 1.5;
        }
        var selectedBasicShapeIds = MaterialTestData.MockBasicShapeIdsForMaterialEdit;

        //Act
        var result = await _controller.Edit(project.Id, materialToUpdate.Id, editedMaterialEditViewModel, selectedBasicShapeIds);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Delete_Get()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var expectedMaterial = project.Materials.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.Delete(project.Id, expectedMaterial.Id);

        //Assert
        result.As<ViewResult>().ViewData.Model.As<Material>().Id.Should().Be(expectedMaterial.Id);
    }

    [Fact]
    [Trait("Delete", "POST")]
    public async Task ValidMaterialId_Throws_NoError_For_Delete_Post()
    {
        //Arrange
        var project = ProjectTestData.MockSimpleProjectWithMaterials;
        var deletedMaterial = project.Materials.First();
        base._mockProjectRepository.Setup(r => r.GetBySpecAsync(It.IsAny<ProjectWithMaterialsSpec>(), TestCancellationToken()))
            .ReturnsAsync(project);

        //Act
        var result = await _controller.DeleteConfirmed(project.Id, deletedMaterial.Id);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

