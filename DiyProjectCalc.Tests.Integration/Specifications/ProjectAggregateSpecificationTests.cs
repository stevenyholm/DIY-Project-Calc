using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.Core.Entities.ProjectAggregate.Specifications;
using DiyProjectCalc.Infrastructure.Data;
using DiyProjectCalc.SharedKernel.Interfaces;
using DiyProjectCalc.TestHelpers.TestData;
using DiyProjectCalc.TestHelpers.TestFixtures;

namespace DiyProjectCalc.Tests.Integration.Specifications;
public class ProjectAggregateSpecificationTests : BaseDatabaseClassFixture
{
    private IRepository<Project> _projectRepository;

    public ProjectAggregateSpecificationTests(DefaultTestDatabaseClassFixture fixture) : base(fixture)
    {
        _projectRepository = new EfRepository<Project>(base.DbContext);
    }

    [Fact]
    [Trait("ProjectWithBasicShapesSpec", "")]
    public async Task ValidProjectId_Returns_BasicShapes_For_ProjectWithBasicShapesSpec()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithBasicShapesSpec(projectId);

        //Act
        var result = await _projectRepository.GetBySpecAsync(projectSpec);

        //Assert
        result.As<Project>().BasicShapes.Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
    }

    [Fact]
    [Trait("ProjectWithMaterialsSpec", "")]
    public async Task ValidProjectId_Returns_Materials_For_ProjectWithMaterialsSpec()
    {
        //Arrange
        var projectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithMaterialsSpec(projectId);

        //Act
        var result = await _projectRepository.GetBySpecAsync(projectSpec);

        //Assert
        result.As<Project>().Materials.Should().HaveCount(ProjectTestData.ValidProjectCountMaterials);
    }

    [Fact]
    [Trait("ProjectWithAggregatesSpec", "")]
    public async Task ValidProjectId_Returns_All_Aggregates_For_ProjectWithAggregatesSpec()
    {
        //Arrange
        var expectedProjectId = ProjectTestData.ValidProjectId(base.DbContext);
        var projectSpec = new ProjectWithAggregatesSpec(expectedProjectId);

        //Act
        var result = await _projectRepository.GetBySpecAsync(projectSpec);

        //Assert
        using (new AssertionScope())
        {
            result.As<Project>().Id.Should().Be(expectedProjectId);
            result.As<Project>().BasicShapes.Should().HaveCount(ProjectTestData.ValidProjectCountBasicShapes);
            result.As<Project>().Materials.Should().HaveCount(ProjectTestData.ValidProjectCountMaterials);
        }
    }

}
