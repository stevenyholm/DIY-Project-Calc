using SUT = DiyProjectCalc.Repositories;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestData;
using System.Collections.Generic;
using System.Linq;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Integration.Repositories;

public class BasicShapeRepositoryTests : BaseDatabaseClassFixture
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
    public async Task ValidObject_Adds_Item_For_AddAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext); 
        var newObject = BasicShapeTestData.NewBasicShape;
        newObject.ProjectId = projectId;
        var beforeCount = base.DbContext.BasicShapes.Count();

        //Act
        await _repository.AddAsync(newObject);

        //Assert
        var afterCount = base.DbContext.BasicShapes.Count();
        afterCount.Should().Be(beforeCount + 1);
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Updates_Item_For_UpdateAsync()
    {
        //Arrange
        var objectToUpdate = BasicShapeTestData.ValidBasicShape(base.DbContext);
        var objectId = default(int);
        if (objectToUpdate is not null)
        {
            objectToUpdate.ShapeType = BasicShapeType.Triangle;
            objectToUpdate.Number1 = 100.1;
            objectToUpdate.Number2 = 200.2;
            objectToUpdate.Name = "edited basic shape";
            objectId = objectToUpdate.BasicShapeId;
        }

        //Act
        await _repository.UpdateAsync(objectToUpdate!);

        //Assert
        var result = base.DbContext.BasicShapes.First(o => o.BasicShapeId == objectId);
        result.As<BasicShape>().Name.Should().Be(objectToUpdate!.Name);
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Removes_Item_For_DeleteAsync()
    {
        //Arrange
        var objectToDelete = BasicShapeTestData.ValidBasicShape(base.DbContext);
        var beforeCount = base.DbContext.BasicShapes.Count();

        //Act
        await _repository.DeleteAsync(objectToDelete!);

        //Assert
        var afterCount = base.DbContext.BasicShapes.Count();
        afterCount.Should().Be(beforeCount - 1);
    }

}
