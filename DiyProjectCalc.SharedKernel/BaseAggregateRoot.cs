using AutoMapper;  
using DiyProjectCalc.SharedKernel.Interfaces;

namespace DiyProjectCalc.SharedKernel;
public class BaseAggregateRoot : BaseEntity, IAggregateRoot
{
    protected T GetAggregate<T>(ICollection<T> collection, int id)
        where T : BaseEntity
    {
        return collection.FirstOrDefault(e => e.Id == id)!;
    }

    protected bool AggregateExists<T>(ICollection<T> collection, int id)
        where T : BaseEntity
    {
        return collection.Any(e => e.Id == id);
    }

    protected void AddAggregate<T>(ICollection<T> collection, T entityToAdd)
        where T : BaseEntity
    {
        collection.Add(entityToAdd);
    }

    protected void UpdateAggregate<T>(ICollection<T> collection, T detachedEntityWithUpdates, IMapper mapper)
        where T : BaseEntity
    {
        var attachedEntityToUpdate = collection.FirstOrDefault(e => e.Id == detachedEntityWithUpdates.Id);
        mapper.Map<T, T>(detachedEntityWithUpdates, attachedEntityToUpdate!);
    }

    protected void RemoveAggregate<T>(ICollection<T> collection, int id)
        where T : BaseEntity
    {
        var entityToRemove = collection.FirstOrDefault(e => e.Id == id);
        collection.Remove(entityToRemove!);
    }

}
