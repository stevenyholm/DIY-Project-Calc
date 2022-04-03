using Ardalis.Specification.EntityFrameworkCore;
using DiyProjectCalc.SharedKernel.Interfaces;

namespace DiyProjectCalc.Infrastructure.Data;
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> 
    where T : class, IAggregateRoot
{

    private ApplicationDbContext _dbContext;
    public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}