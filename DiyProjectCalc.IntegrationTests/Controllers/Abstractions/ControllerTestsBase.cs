using DiyProjectCalc.Data;
using DiyProjectCalc.IntegrationTests.TestFixtures;
using DiyProjectCalc.Tests.TestData;
using System.Linq;
using Xunit;

namespace DiyProjectCalc.IntegrationTests.Controllers.Abstractions;

public abstract class ControllerTestsBase : IClassFixture<DefaultTestDatabaseFixture>
{
    public DefaultTestDatabaseFixture Fixture { get; }

    protected ControllerTestsBase(DefaultTestDatabaseFixture fixture) => Fixture = fixture;

    protected ApplicationDbContext NewDbContext() => Fixture.CreateContext();

    protected int ValidMaterialId(ApplicationDbContext dbContext) =>
        dbContext.Materials.FirstOrDefault(m => m.Name == MaterialsTestData.NameValid)?.MaterialId ?? 0;

    protected int ValidProjectId(ApplicationDbContext dbContext) =>
        dbContext.Projects.FirstOrDefault(m => m.Name == ProjectsTestData.NameValid)?.ProjectId ?? 0;

    protected int ValidBasicShapeId(ApplicationDbContext dbContext) =>
        dbContext.BasicShapes.FirstOrDefault(m => m.Name == BasicShapesTestData.NameValid)?.BasicShapeId ?? 0;
}
