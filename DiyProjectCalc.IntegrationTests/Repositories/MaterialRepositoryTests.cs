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
    public MaterialRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("GetMaterialAsync", "")]
    public async Task ValidMaterialId_Returns_CorrectObject_For_GetMaterialAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFMaterialRepository(dbContext);
        int expectedId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await repository.GetMaterialAsync(expectedId);

        //Assert
        result.As<Material>().MaterialId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetMaterialsForProjectAsync", "")]
    public async Task ValidProjectId_Returns_Materials_For_GetMaterialsForProjectAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFMaterialRepository(dbContext);
        var projectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await repository.GetMaterialsForProjectAsync(projectId);

        //Assert
        result.As<IEnumerable<Material>>().Should().HaveCount(ProjectTestData.ValidProjectCountMaterials);
    }

    [Fact]
    [Trait("MaterialExists", "")]
    public async Task ValidMaterialId_Returns_True_For_MaterialExists()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFMaterialRepository(dbContext);
        int expectedId = MaterialTestData.ValidMaterialId(dbContext);

        //Act
        var result = await repository.MaterialExists(expectedId);

        //Assert
        result.Should().Be(true);
    }

    [Fact]
    [Trait("AddAsync", "")]
    public async Task ValidObject_Throws_NoError_For_AddAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new SUT.EFMaterialRepository(dbContext);
        var projectId = ProjectTestData.ValidProjectId(dbContext);
        var newObject = MaterialTestData.NewMaterial;
        newObject.ProjectId = projectId;

        //Act
        await repository.AddAsync(newObject);
        dbContext.ChangeTracker.Clear();

        //Assert
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Throws_NoError_For_UpdateAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new SUT.EFMaterialRepository(dbContext);
        var objectToUpdate = MaterialTestData.ValidMaterial(dbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.Name = "edited basic shape";
            objectToUpdate.MeasurementType = MaterialMeasurement.Area;
            objectToUpdate.Length = 99.1;
            objectToUpdate.Depth = 88.1;
            objectToUpdate.Width = 77.1;
        }
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(dbContext);

        //Act
        await repository.UpdateAsync(objectToUpdate!, newSelectedBasicShapeIds);
        dbContext.ChangeTracker.Clear();

        //Assert
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Throws_NoError_For_DeleteAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new SUT.EFMaterialRepository(dbContext);
        var objectToDelete = MaterialTestData.ValidMaterial(dbContext);

        //Act
        await repository.DeleteAsync(objectToDelete!);
        dbContext.ChangeTracker.Clear();

        //Assert
    }
}
