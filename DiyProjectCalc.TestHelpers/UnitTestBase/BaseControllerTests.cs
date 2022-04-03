using System.Threading;
using Moq;
using DiyProjectCalc.Core.Entities.ProjectAggregate;
using DiyProjectCalc.SharedKernel.Interfaces;

namespace DiyProjectCalc.TestHelpers.UnitTestBase;
public abstract class BaseControllerTests 
{
    protected Mock<IRepository<Project>> _mockProjectRepository = new Mock<IRepository<Project>>();

    protected CancellationToken TestCancellationToken() => new CancellationToken();

}
