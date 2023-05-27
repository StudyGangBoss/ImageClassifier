using Ardalis.Specification.EntityFrameworkCore;

namespace Infrastructure;

public class ReadRepository<T> : RepositoryBase<T>, IReadRepository<T> where T : class
{
    public ReadRepository(ImageClassifierContext dbContext) : base(dbContext)
    {
    }
}