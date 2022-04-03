using Ardalis.Specification;

namespace DiyProjectCalc.SharedKernel.Interfaces;
public interface IReadRepository<T> : IReadRepositoryBase<T> 
    where T : class, IAggregateRoot
{
}
