using DiyProjectCalc.Data;
using Xunit;

namespace DiyProjectCalc.IntegrationTests.TestFixtures;

public abstract class BaseClassFixture : IClassFixture<DefaultTestDatabaseClassFixture>
{
    public DefaultTestDatabaseClassFixture Fixture { get; }

    public BaseClassFixture(DefaultTestDatabaseClassFixture fixture) => Fixture = fixture;

    protected ApplicationDbContext NewDbContext() => Fixture.CreateContext();
}
