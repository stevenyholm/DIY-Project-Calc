using SUT = DiyProjectCalc.Repositories;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using Xunit;
using DiyProjectCalc.Models;
using FluentAssertions;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestData;
using System.Collections.Generic;

namespace DiyProjectCalc.IntegrationTests.Repositories;

public class BasicShapeRepositoryTests : BaseClassFixture
{
    private SUT.EFBasicShapeRepository _repository;
    public BasicShapeRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    { 
        _repository = new SUT.EFBasicShapeRepository(base.DbContext);
    }

    [Fact]
    [Trait("GetBasicShapeAsync", "")]
    public async Task ValidBasicShapeId_Returns_CorrectObject_For_GetBasicShapeAsync()
    {
        //Arrange
        int expectedId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _repository.GetBasicShapeAsync(expectedId);

        //Assert
        result.As<BasicShape>().BasicShapeId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetBasicShapesForProjectAsync", "")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetBasicShapesForProjectAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _repository.GetBasicShapesForProjectAsync(projectId);

        //Assert
        result.As<IEnumerable<BasicShape>>().Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
    }

    [Fact]
    [Trait("BasicShapeExists", "")]
    public async Task ValidBasicShapeId_Returns_True_For_BasicShapeExists()
    {
        //Arrange
        int expectedId = BasicShapeTestData.ValidBasicShapeId(base.DbContext);

        //Act
        var result = await _repository.BasicShapeExists(expectedId);

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
        var newObject = BasicShapeTestData.NewBasicShape;
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
        var objectToUpdate = BasicShapeTestData.ValidBasicShape(base.DbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.ShapeType = BasicShapeType.Triangle;
            objectToUpdate.Number1 = 100.1;
            objectToUpdate.Number2 = 200.2;
            objectToUpdate.Name = "edited basic shape";
        }

        //Act
        await _repository.UpdateAsync(objectToUpdate!);
        base.RollbackTransaction(base.DbContext);

        //Assert
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Throws_NoError_For_DeleteAsync()
    {
        //Arrange
        base.BeginTransaction(base.DbContext);
        var objectToDelete = BasicShapeTestData.ValidBasicShape(base.DbContext);

        //Act
        await _repository.DeleteAsync(objectToDelete!);
        base.RollbackTransaction(base.DbContext);

        //Assert
    }

}
