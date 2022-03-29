using DiyProjectCalc.Infrastructure.Data;
using System;
using Xunit;

namespace DiyProjectCalc.TestHelpers.TestFixtures;

public abstract class BaseDatabaseClassFixture : IClassFixture<DefaultTestDatabaseClassFixture>, IDisposable
{
    public DefaultTestDatabaseClassFixture Fixture { get; }
    protected ApplicationDbContext DbContext { get; }

    public BaseDatabaseClassFixture(DefaultTestDatabaseClassFixture fixture)
    {
        Fixture = fixture;
        DbContext = Fixture.CreateContext();
        DbContext.Database.BeginTransaction();
    }

    public virtual void Dispose()
    {
        DbContext.Database.RollbackTransaction();
        DbContext.Dispose();
    }
}
