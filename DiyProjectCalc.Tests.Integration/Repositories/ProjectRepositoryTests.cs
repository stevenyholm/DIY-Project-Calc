using SUT = DiyProjectCalc.Infrastructure;
using DiyProjectCalc.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using DiyProjectCalc.TestHelpers.TestData;
using FluentAssertions;
using System.Linq;
using DiyProjectCalc.TestHelpers.TestFixtures;
using DiyProjectCalc.Core.Entities.ProjectAggregate;

namespace DiyProjectCalc.Tests.Integration.Repositories;

public class ProjectRepositoryTests : BaseDatabaseClassFixture
{
    private IRepository<Project> _projectRepository;
    public ProjectRepositoryTests(DefaultTestDatabaseClassFixture fixture) : base(fixture) 
    {
        _projectRepository = new SUT.Data.EfRepository<Project>(base.DbContext);
    }

    [Fact]
    [Trait("ListAsync", "")]
    public async Task Returns_Projects_For_ListAsync()
    {
        //Arrange
        var expectedCount = ProjectTestData.ProjectsCount(base.DbContext);

        //Act
        var result = await _projectRepository.ListAsync();

        //Assert
        result.As<IEnumerable<Project>>().Should().HaveCount(expectedCount);
    }

    [Fact]
    [Trait("GetByIdAsync", "")]
    public async Task ValidProjectId_Returns_CorrectObject_For_GetByIdAsync()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);

        //Act
        var result = await _projectRepository.GetByIdAsync<int>(expectedProjectId);

        //Assert
        result.As<Project>().Id.Should().Be(expectedProjectId);
    }

    [Fact]
    [Trait("AddAsync", "")]
    public async Task ValidObject_Adds_Item_For_AddAsync()
    {
        //Arrange
        var newProject = ProjectTestData.NewProject;
        var expectedCount = ProjectTestData.ProjectsCount(base.DbContext) + 1;

        //Act
        await _projectRepository.AddAsync(newProject);

        //Assert
        var afterCount = ProjectTestData.ProjectsCount(base.DbContext);
        afterCount.Should().Be(expectedCount);
    }

    [Fact]
    [Trait("UpdateAsync", "")]
    public async Task ValidObject_Updates_Item_For_UpdateAsync()
    {
        //Arrange
        var editedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedProjectName = "edited project";
        if (editedProject is not null)
        {
            editedProject.Name = expectedProjectName;
        }

        //Act
        await _projectRepository.UpdateAsync(editedProject!);

        //Assert
        var result = ProjectTestData.ValidProject(base.DbContext, editedProject!.Id); 
        result.As<Project>().Name.Should().Be(expectedProjectName);
    }

    [Fact]
    [Trait("DeleteAsync", "")]
    public async Task ValidObject_Removes_Item_For_DeleteAsync()
    {
        //Arrange
        var deletedProject = ProjectTestData.ValidProject(base.DbContext);
        var expectedCount = ProjectTestData.ProjectsCount(base.DbContext) - 1;

        //Act
        await _projectRepository.DeleteAsync(deletedProject!);

        //Assert
        var afterCount = ProjectTestData.ProjectsCount(base.DbContext);
        afterCount.Should().Be(expectedCount);
    }
}
