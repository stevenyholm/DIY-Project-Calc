using DiyProjectCalc.Data;
using System;
using Xunit;

namespace DiyProjectCalc.IntegrationTests.TestFixtures;

public abstract class BaseClassFixture : IClassFixture<DefaultTestDatabaseClassFixture>, IDisposable
{
    public DefaultTestDatabaseClassFixture Fixture { get; }
    protected ApplicationDbContext DbContext { get; }

    public BaseClassFixture(DefaultTestDatabaseClassFixture fixture)
    {
        Fixture = fixture;
        DbContext = Fixture.CreateContext();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }

    protected void BeginTransaction(ApplicationDbContext dbContext) =>
        dbContext.Database.BeginTransaction();

    protected void RollbackTransaction(ApplicationDbContext dbContext) =>
        dbContext.ChangeTracker.Clear();
}
