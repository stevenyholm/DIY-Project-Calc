using DiyProjectCalc.Data;
using DiyProjectCalc.TestHelpers.TestData;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.IntegrationTests.TestFixtures;

public class DefaultTestDatabaseClassFixture
{
    private const string ConnectionString = @"Server=steve-dell;Database=DiyProjectCalcTests;Trusted_Connection=true;MultipleActiveResultSets=True";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public DefaultTestDatabaseClassFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    seedInitialTestData(context);
                }

                _databaseInitialized = true;
            }
        }
    }

    public ApplicationDbContext CreateContext()
        => new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

    private void seedInitialTestData(ApplicationDbContext dbContext)
    {
        var projectsTestData = new ProjectTestData();
        var projects = ProjectTestData.ProjectsFor(projectsTestData.ValidProjectTestModelList);
        dbContext.Projects.AddRange(projects);

        dbContext.SaveChanges();
    }
}