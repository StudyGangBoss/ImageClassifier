using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure;

public class Repository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public Repository(ImageClassifierContext dbContext) : base(dbContext)
    {
    }
}