using DiyProjectCalc.Data;
using DiyProjectCalc.Tests.TestData;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DiyProjectCalc.Tests.Controllers.Abstractions;

public abstract class ControllerTestsBase
{
    private DbContextOptions<ApplicationDbContext> ContextOptions { get; }

    protected ControllerTestsBase()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                        //.UseSqlServer("Server=steve-dell;Database=DiyProjectCalcTests;Trusted_Connection=true")
                        .UseSqlite(connection)  //needs package Microsoft.EntityFrameworkCore.Sqlite
                        //.UseInMemoryDatabase("DiyProjectCalcTestDatabase")  //needs Package Microsoft.EntityFrameworkCore.InMemory
                        .Options;

        using (var context = NewDbContext())
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            seedInitialTestData(context);
        }
    }


    protected ApplicationDbContext NewDbContext() => new ApplicationDbContext(ContextOptions);

    protected int ValidMaterialId(ApplicationDbContext dbContext) =>
        dbContext.Materials.FirstOrDefault(m => m.Name == MaterialsTestData.NameValid)?.MaterialId ?? 0;

    protected int ValidProjectId(ApplicationDbContext dbContext) =>
        dbContext.Projects.FirstOrDefault(m => m.Name == ProjectsTestData.NameValid)?.ProjectId ?? 0;

    protected int ValidBasicShapeId(ApplicationDbContext dbContext) =>
    dbContext.BasicShapes.FirstOrDefault(m => m.Name == BasicShapesTestData.NameValid)?.BasicShapeId ?? 0;


    private void seedInitialTestData(ApplicationDbContext dbContext)
    {
        var projectsTestData = new ProjectsTestData();
        var projects = ProjectsTestData.ProjectsFor(projectsTestData.TestDataForAllProjects);
        dbContext.Projects.AddRange(projects);

        dbContext.SaveChanges();
    }

}
