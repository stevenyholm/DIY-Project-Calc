using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using SUT = DiyProjectCalc.Infrastructure;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.TestHelpers.Helpers;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestFixtures;

namespace DiyProjectCalc.Tests.Integration.Repositories;

public class MaterialRepositoryTests : BaseDatabaseClassFixture
{
    private IRepository<Project> _projectRepository;

    public MaterialRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _projectRepository = new SUT.Data.EfRepository<Project>(base.DbContext);
    }

    [Fact]
    [Trait("GetBySpecAsync", "")]
    public async Task ValidProjectId_Returns_Materials_For_GetBySpecAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var expectedCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, projectId);

        //Act
        var projectSpec = new ProjectWithMaterialsSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var result = project?.Materials;

        //Assert
        result.As<IEnumerable<Material>>().Should().HaveCount(expectedCount);
    }

    [Fact]
    [Trait("GetMaterial", "")]
    public async Task ValidMaterialId_Returns_CorrectObject_For_GetMaterial()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithMaterialsSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var expectedMaterial = project?.Materials.First();

        //Act
        var result = project?.GetMaterial(expectedMaterial!.Id);

        //Assert
        result.As<Material>().Id.Should().Be(expectedMaterial!.Id);
    }

    [Fact]
    [Trait("AddMaterial", "")]
    public async Task ValidObject_Adds_Item_For_AddMaterial()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithMaterialsSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var newMaterial = MaterialTestData.NewMaterial;
        newMaterial.ProjectId = project!.Id;
        var beforeCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, projectId);

        //Act
        project?.AddMaterial(newMaterial);
        await _projectRepository.SaveChangesAsync();

        //Assert
        var afterCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, projectId);
        afterCount.Should().Be(beforeCount + 1);
    }

    [Fact]
    [Trait("UpdateMaterial", "")]
    public async Task ValidObject_Updates_Item_For_UpdateMaterial()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithMaterialsSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var editedMaterial = project?.Materials.First();
        var expectedMaterialName = "edited basic shape";
        if (editedMaterial is not null)
        {
            editedMaterial.Name = expectedMaterialName;
            editedMaterial.MeasurementType = MaterialMeasurement.Area;
            editedMaterial.Length = 99.1;
            editedMaterial.Depth = 88.1;
            editedMaterial.Width = 77.1;
        }

        //Act
        project?.UpdateMaterial(editedMaterial!, MapperHelper.CreateMapper());
        await _projectRepository.SaveChangesAsync();

        //Assert
        var result = MaterialTestData.ValidMaterial(base.DbContext, editedMaterial!.Id);
        result.As<Material>().Name.Should().Be(expectedMaterialName);
    }

    [Fact]
    [Trait("RemoveMaterial", "")]
    public async Task ValidObject_Removes_Item_For_RemoveMaterial()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithMaterialsSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var deletedMaterial = MaterialTestData.ValidMaterial(base.DbContext);
        var beforeCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, projectId);

        //Act
        project?.RemoveMaterial(deletedMaterial!.Id);
        await _projectRepository.SaveChangesAsync();

        //Assert
        var afterCount = ProjectTestData.ProjectMaterialsCount(base.DbContext, projectId);
        afterCount.Should().Be(beforeCount - 1);
    }
}
