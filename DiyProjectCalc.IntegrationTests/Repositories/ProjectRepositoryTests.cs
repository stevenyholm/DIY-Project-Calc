using SUT = DiyProjectCalc.Repositories;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.Models;
using FluentAssertions;

namespace DiyProjectCalc.IntegrationTests.Repositories;

public class ProjectRepositoryTests : BaseClassFixture
{
    public ProjectRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) { }

    [Fact]
    [Trait("GetProjectAsync", "")]
    public async Task ValidProjectId_Returns_CorrectObject_For_GetProjectAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFProjectRepository(dbContext);
        var expectedId = ProjectTestData.ValidProjectId(dbContext);

        //Act
        var result = await repository.GetProjectAsync(expectedId);

        //Assert
        result.As<Project>().ProjectId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetAllProjectsAsync", "")]
    public async Task Returns_Projectss_For_GetAllProjectsAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        var repository = new SUT.EFProjectRepository(dbContext);

        //Act
        var result = await repository.GetAllProjectsAsync();

        //Assert
        result.As<IEnumerable<Project>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("AddAsync", "")]
    public async Task ValidObject_Throws_NoError_For_AddAsync()
    {
        //Arrange
        using var dbContext = base.NewDbContext();
        dbContext.Database.BeginTransaction();
        var repository = new SUT.EFProjectRepository(dbContext);
        var newObject = ProjectTestData.NewProject;

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
        var repository = new SUT.EFProjectRepository(dbContext);
        var objectToUpdate = ProjectTestData.ValidProject(dbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.Name = "edited project";
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
        var repository = new SUT.EFProjectRepository(dbContext);
        var objectToDelete = ProjectTestData.ValidProject(dbContext);

        //Act
        await repository.DeleteAsync(objectToDelete!);
        dbContext.ChangeTracker.Clear();

        //Assert
    }
}
