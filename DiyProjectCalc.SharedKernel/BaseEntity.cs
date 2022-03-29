namespace DiyProjectCalc.SharedKernel;
public abstract class BaseEntity
{
    //TODO: Clean Architecture also used this: public virtual int Id { get; protected set; }
    public int Id { get; set; }

    //TODO: Clean Architecture also used this: public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
}