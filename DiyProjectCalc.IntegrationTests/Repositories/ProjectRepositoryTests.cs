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
    private SUT.EFProjectRepository _repository;
    public ProjectRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _repository = new SUT.EFProjectRepository(base.DbContext);
    }

    [Fact]
    [Trait("GetProjectAsync", "")]
    public async Task ValidProjectId_Returns_CorrectObject_For_GetProjectAsync()
    {
        //Arrange
        var expectedId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _repository.GetProjectAsync(expectedId);

        //Assert
        result.As<Project>().ProjectId.Should().Be(expectedId);
    }

    [Fact]
    [Trait("GetAllProjectsAsync", "")]
    public async Task Returns_Projectss_For_GetAllProjectsAsync()
    {
        //Arrange

        //Act
        var result = await _repository.GetAllProjectsAsync();

        //Assert
        result.As<IEnumerable<Project>>().Should().HaveCount(ProjectTestData.ValidProjectListCount);
    }

    [Fact]
    [Trait("AddAsync", "")]
    public async Task ValidObject_Throws_NoError_For_AddAsync()
    {
        //Arrange
        var newObject = ProjectTestData.NewProject;

        //Act
        await _repository.AddAsync(newObject);

        //Assert
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Throws_NoError_For_UpdateAsync()
    {
        //Arrange
        var objectToUpdate = ProjectTestData.ValidProject(base.DbContext);
        if (objectToUpdate is not null)
        {
            objectToUpdate.Name = "edited project";
        }

        //Act
        await _repository.UpdateAsync(objectToUpdate!);

        //Assert
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Throws_NoError_For_DeleteAsync()
    {
        //Arrange
        var objectToDelete = ProjectTestData.ValidProject(base.DbContext);

        //Act
        await _repository.DeleteAsync(objectToDelete!);

        //Assert
    }
}
