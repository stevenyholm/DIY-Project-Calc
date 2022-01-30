using SUT = DiyProjectCalc.Repositories;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace DiyProjectCalc.IntegrationTests.Repositories;

public class MaterialRepositoryTests : BaseClassFixture
{
    private SUT.EFMaterialRepository _repository;
    public MaterialRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _repository = new SUT.EFMaterialRepository(base.DbContext);
    }

    [Fact]
    [Trait("GetMaterialAsync", "")]
    public async Task ValidMaterialId_Returns_CorrectObject_For_GetMaterialAsync()
    {
        //Arrange
        int expectedId = MaterialTestData.ValidMaterialId(base.DbContext);

        //Act
        var result = await _repository.GetMaterialAsync(expectedId);

        //Assert
        result.As<Material>().MaterialId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetMaterialsForProjectAsync", "")]
    public async Task ValidProjectId_Returns_Materials_For_GetMaterialsForProjectAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _repository.GetMaterialsForProjectAsync(projectId);

        //Assert
        result.As<IEnumerable<Material>>().Should().HaveCount(ProjectTestData.ValidProjectCountMaterials);
    }

    [Fact]
    [Trait("MaterialExists", "")]
    public async Task ValidMaterialId_Returns_True_For_MaterialExists()
    {
        //Arrange
        int expectedId = MaterialTestData.ValidMaterialId(base.DbContext);

        //Act
        var result = await _repository.MaterialExists(expectedId);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    [Trait("AddAsync", "")]
    public async Task ValidObject_Throws_NoError_For_AddAsync()
    {
        //Arrange
        base.BeginTransaction(base.DbContext);
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newObject = MaterialTestData.NewMaterial;
        newObject.ProjectId = projectId;

        //Act
        await _repository.AddAsync(newObject);
        base.RollbackTransaction(base.DbContext);

        //Assert
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Throws_NoError_For_UpdateAsync()
    {
        //Arrange
        base.BeginTransaction(base.DbContext);
        var objectToUpdate = MaterialTestData.ValidMaterial(base.DbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.Name = "edited basic shape";
            objectToUpdate.MeasurementType = MaterialMeasurement.Area;
            objectToUpdate.Length = 99.1;
            objectToUpdate.Depth = 88.1;
            objectToUpdate.Width = 77.1;
        }
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(base.DbContext);

        //Act
        await _repository.UpdateAsync(objectToUpdate!, newSelectedBasicShapeIds);
        base.RollbackTransaction(base.DbContext);

        //Assert
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Throws_NoError_For_DeleteAsync()
    {
        //Arrange
        base.BeginTransaction(base.DbContext);
        var objectToDelete = MaterialTestData.ValidMaterial(base.DbContext);

        //Act
        await _repository.DeleteAsync(objectToDelete!);
        base.RollbackTransaction(base.DbContext);

        //Assert
    }
}
