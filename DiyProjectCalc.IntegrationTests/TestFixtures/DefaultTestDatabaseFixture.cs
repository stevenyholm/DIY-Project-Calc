using DiyProjectCalc.Data;
using DiyProjectCalc.Tests.TestData;
using Microsoft.EntityFrameworkCore;

namespace DiyProjectCalc.IntegrationTests.TestFixtures;

public class DefaultTestDatabaseFixture
{
    private const string ConnectionString = @"Server=steve-dell;Database=DiyProjectCalcTests;Trusted_Connection=true";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public DefaultTestDatabaseFixture()
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
        var projectsTestData = new ProjectsTestData();
        var projects = ProjectsTestData.ProjectsFor(projectsTestData.TestDataForAllProjects);
        dbContext.Projects.AddRange(projects);

        dbContext.SaveChanges();
    }
}