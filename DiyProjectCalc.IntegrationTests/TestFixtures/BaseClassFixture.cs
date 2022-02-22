using DiyProjectCalc.Data;
using Microsoft.EntityFrameworkCore.Storage;
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
        DbContext.Database.BeginTransaction();

    }

    public void Dispose()
    {
        DbContext.Database.RollbackTransaction();
        DbContext.Dispose();
    }
}
