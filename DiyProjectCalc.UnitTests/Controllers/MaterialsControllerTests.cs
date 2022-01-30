using SUT = DiyProjectCalc.Controllers;
using DiyProjectCalc.Models;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.ViewModels;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DiyProjectCalc.Repositories;

namespace DiyProjectCalc.UnitTests.Controllers;

[Collection("Controllers")]
public class MaterialsControllerTests 
{
    [Fact]
    [Trait("Index", "GET")]
    public async Task ValidProjectId_Returns_Materials_For_Index_Get()
    {
        //Arrange
        var project = new ProjectTestData().ValidProjectTestModel.Project;
        var mockProjectRepository = new Mock<IProjectRepository>();
        mockProjectRepository.Setup(r => r.GetProjectAsync(It.IsAny<int>())).ReturnsAsync(project);
        var mockRepository = new Mock<IMaterialRepository>();
        mockRepository.Setup(r => r.GetMaterialsForProjectAsync(It.IsAny<int>())).ReturnsAsync(project.Materials);
        var controller = new SUT.MaterialsController(mockRepository.Object, mockProjectRepository.Object, null!);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

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
        var material = MaterialTestData.MockSimpleMaterial;
        var expectedMaterialId = MaterialTestData.MockSimpleMaterialId;
        var mockRepository = new Mock<IMaterialRepository>();
        mockRepository.Setup(r => r.GetMaterialAsync(It.IsAny<int>())).ReturnsAsync(material);
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, null!);

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
        var mockRepository = new Mock<IMaterialRepository>();
        var mockBasicShapeRepository = new Mock<IBasicShapeRepository>();
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, mockBasicShapeRepository.Object);
        var expectedProjectId = ProjectTestData.MockSimpleProjectId;

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
        var mockRepository = new Mock<IMaterialRepository>();
        var mockBasicShapeRepository = new Mock<IBasicShapeRepository>();
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, mockBasicShapeRepository.Object);
        var projectId = ProjectTestData.MockSimpleProjectId;
        var newMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = projectId,
            Material = MaterialTestData.NewMaterial
        };
        var newSelectedBasicShapeIds = MaterialTestData.MockBasicShapeIdsForMaterialEdit;

        //Act
        var result = await controller.Create(newMaterialEditViewModel, newSelectedBasicShapeIds); 

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Edit", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Edit_Get()
    {
        //Arrange
        var mockRepository = new Mock<IMaterialRepository>();
        var material = MaterialTestData.MockSimpleMaterial;
        material.ProjectId = ProjectTestData.MockSimpleProjectId;
        mockRepository.Setup(r => r.GetMaterialAsync(It.IsAny<int>())).ReturnsAsync(material);
        var mockBasicShapeRepository = new Mock<IBasicShapeRepository>();
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, mockBasicShapeRepository.Object);
        var expectedMaterialId = MaterialTestData.MockSimpleMaterialId;

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
        var mockRepository = new Mock<IMaterialRepository>();
        var mockBasicShapeRepository = new Mock<IBasicShapeRepository>();
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, mockBasicShapeRepository.Object);
        var materialId = MaterialTestData.MockSimpleMaterialId;
        var projectId = ProjectTestData.MockSimpleProjectId;
        var editMaterialEditViewModel = new MaterialEditViewModel()
        {
            ProjectId = projectId,
            Material = MaterialTestData.MockMaterialWithBasicShapes 
        };
        if (editMaterialEditViewModel.Material is not null)
        {
            editMaterialEditViewModel.Material.MaterialId = materialId;
            editMaterialEditViewModel.Material.Name = "2x4 redwood";
            editMaterialEditViewModel.Material.MeasurementType = MaterialMeasurement.Linear;
            editMaterialEditViewModel.Material.Length = 8.0;
            editMaterialEditViewModel.Material.Width = 3.5;
            editMaterialEditViewModel.Material.Depth = 1.5;
        }
        var newSelectedBasicShapeIds = MaterialTestData.MockBasicShapeIdsForMaterialEdit;

        //Act
        var result = await controller.Edit(materialId, editMaterialEditViewModel, newSelectedBasicShapeIds);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }

    [Fact]
    [Trait("Delete", "GET")]
    public async Task ValidMaterialId_Returns_Material_For_Delete_Get()
    {
        //Arrange
        var material = MaterialTestData.MockSimpleMaterial;
        var mockRepository = new Mock<IMaterialRepository>();
        mockRepository.Setup(r => r.GetMaterialAsync(It.IsAny<int>())).ReturnsAsync(material);
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, null!);
        var expectedMaterialId = MaterialTestData.MockSimpleMaterialId;

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
        var mockRepository = new Mock<IMaterialRepository>();
        var controller = new SUT.MaterialsController(mockRepository.Object, null!, null!);
        var materialId = MaterialTestData.MockSimpleMaterialId;

        //Act
        var result = await controller.DeleteConfirmed(materialId);

        //Assert
        result.Should().BeOfType<RedirectToActionResult>();
    }
}

