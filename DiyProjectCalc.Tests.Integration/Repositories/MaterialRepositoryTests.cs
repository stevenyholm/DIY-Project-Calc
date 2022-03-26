using SUT = DiyProjectCalc.Repositories;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Integration.Repositories;

public class MaterialRepositoryTests : BaseDatabaseClassFixture
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
    public async Task ValidObject_Adds_Item_For_AddAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var newObject = MaterialTestData.NewMaterial;
        newObject.ProjectId = projectId;
        var beforeCount = base.DbContext.Materials.Count();

        //Act
        await _repository.AddAsync(newObject);

        //Assert
        var afterCount = base.DbContext.Materials.Count();
        afterCount.Should().Be(beforeCount + 1);
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Updates_Item_For_UpdateAsync()
    {
        //Arrange
        var objectToUpdate = MaterialTestData.ValidMaterial(base.DbContext);
        var objectId = default(int);
        if (objectToUpdate is not null)
        {
            objectToUpdate.Name = "edited basic shape";
            objectToUpdate.MeasurementType = MaterialMeasurement.Area;
            objectToUpdate.Length = 99.1;
            objectToUpdate.Depth = 88.1;
            objectToUpdate.Width = 77.1;
            objectId = objectToUpdate.MaterialId;
        }
        var newSelectedBasicShapeIds = MaterialTestData.ValidNewSelectedBasicShapeIds(base.DbContext);

        //Act
        await _repository.UpdateAsync(objectToUpdate!, newSelectedBasicShapeIds);

        //Assert
        var result = base.DbContext.Materials.First(o => o.MaterialId == objectId);
        result.As<Material>().Name.Should().Be(objectToUpdate!.Name);
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Removes_Item_For_DeleteAsync()
    {
        //Arrange
        var objectToDelete = MaterialTestData.ValidMaterial(base.DbContext);
        var beforeCount = base.DbContext.Materials.Count();

        //Act
        await _repository.DeleteAsync(objectToDelete!);

        //Assert
        var afterCount = base.DbContext.Materials.Count();
        afterCount.Should().Be(beforeCount - 1);
    }
}
