using Ardalis.Specification;

namespace DiyProjectCalc.SharedKernel.Interfaces;
public interface IRepository<T> : IRepositoryBase<T> 
    where T : class, IAggregateRoot
{
}