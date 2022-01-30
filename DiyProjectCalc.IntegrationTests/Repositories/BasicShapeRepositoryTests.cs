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
    public BasicShapeRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("GetBasicShapeAsync", "")]
    public async Task ValidBasicShapeId_Returns_CorrectObject_For_GetBasicShapeAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        int expectedId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await repository.GetBasicShapeAsync(expectedId);

        //Assert
        result.As<BasicShape>().BasicShapeId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetBasicShapesForProjectAsync", "")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetBasicShapesForProjectAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        var projectId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await repository.GetBasicShapesForProjectAsync(projectId);

        //Assert
        result.As<IEnumerable<BasicShape>>().Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
    }

    [Fact]
    [Trait("BasicShapeExists", "")]
    public async Task ValidBasicShapeId_Returns_True_For_BasicShapeExists()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        int expectedId = BasicShapeTestData.ValidBasicShapeId(dbContext);

        //Act
        var result = await repository.BasicShapeExists(expectedId);

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
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        var projectId = ProjectTestData.ValidProjectId(dbContext);
        var newObject = BasicShapeTestData.NewBasicShape;
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
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        var objectToUpdate = BasicShapeTestData.ValidBasicShape(dbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.ShapeType = BasicShapeType.Triangle;
            objectToUpdate.Number1 = 100.1;
            objectToUpdate.Number2 = 200.2;
            objectToUpdate.Name = "edited basic shape";
        }

        //Act
        await repository.UpdateAsync(objectToUpdate!);
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
        var repository = new SUT.EFBasicShapeRepository(dbContext);
        var objectToDelete = BasicShapeTestData.ValidBasicShape(dbContext);

        //Act
        await repository.DeleteAsync(objectToDelete!);
        dbContext.ChangeTracker.Clear();

        //Assert
    }

}
