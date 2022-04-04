using SUT = DiyProjectCalc.Infrastructure;
using DiyProjectCalc.SharedKernel.Interfaces;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using DiyProjectCalc.TestHelpers.TestData;
using System.Collections.Generic;
using System.Linq;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using DiyProjectCalc.TestHelpers.Helpers;

namespace DiyProjectCalc.Tests.Integration.Repositories;

public class BasicShapeRepositoryTests : BaseDatabaseClassFixture
{
    private IRepository<Project> _projectRepository;

    public BasicShapeRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    { 
        _projectRepository = new SUT.Data.EfRepository<Project>(base.DbContext);

    }

    [Fact]
    [Trait("GetBySpecAsync", "")]
    public async Task ValidProjectId_Returns_BasicShapes_For_GetBySpecAsync()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var expectedCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, projectId);

        //Act
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var result = project?.BasicShapes;

        //Assert
        result.As<IEnumerable<BasicShape>>().Should().HaveCount(expectedCount);
    }

    [Fact]
    [Trait("GetBasicShape", "")]
    public async Task ValidBasicShapeId_Returns_CorrectObject_For_GetBasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var expectedBasicShape = project?.BasicShapes.First();

        //Act
        var result = project?.GetBasicShape(expectedBasicShape!.Id);

        //Assert
        result.As<BasicShape>().Id.Should().Be(expectedBasicShape!.Id);
    }

    [Fact]
    [Trait("AddBasicShape", "")]
    public async Task ValidObject_Adds_Item_For_AddBasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var newBasicShape = BasicShapeTestData.NewBasicShape;
        var beforeCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, projectId);

        //Act
        project?.AddBasicShape(newBasicShape);
        await _projectRepository.SaveChangesAsync();

        //Assert
        var afterCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, projectId);
        afterCount.Should().Be(beforeCount + 1);
    }

    [Fact]
    [Trait("UpdateBasicShape", "")]
    public async Task ValidObject_Updates_Item_For_UpdateBasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var editedBasicShape = project?.BasicShapes.First();
        var expectedName = "edited basic shape";
        if (editedBasicShape is not null)
        {
            editedBasicShape.ShapeType = BasicShapeType.Triangle;
            editedBasicShape.Number1 = 100.1;
            editedBasicShape.Number2 = 200.2;
            editedBasicShape.Name = expectedName;
        }

        //Act
        project?.UpdateBasicShape(editedBasicShape!, MapperHelper.CreateMapper());
        await _projectRepository.SaveChangesAsync();

        //Assert
        var result = BasicShapeTestData.ValidBasicShape(base.DbContext, editedBasicShape!.Id);
        result.As<BasicShape>().Name.Should().Be(expectedName);
    }

    [Fact]
    [Trait("RemoveBasicShape", "")]
    public async Task ValidObject_Removes_Item_For_RemoveBasicShape()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);
        var project = await _projectRepository.GetBySpecAsync(projectSpec);
        var deletedBasicShape = project?.BasicShapes.First();
        var beforeCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, projectId);

        //Act
        project?.RemoveBasicShape(deletedBasicShape!.Id);
        await _projectRepository.SaveChangesAsync();

        //Assert
        var afterCount = ProjectTestData.ProjectBasicShapesCount(base.DbContext, projectId);
        afterCount.Should().Be(beforeCount - 1);
    }

}
